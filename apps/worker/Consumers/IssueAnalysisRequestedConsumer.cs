using FixFlow.Contracts.Events.Issues;
using FixFlow.Worker.Abstractions;
using MassTransit;

namespace FixFlow.Worker.Consumers;

public class IssueAnalysisRequestedConsumer(
    IAiIssueAnalyzer issueAnalyzer,
    ILogger<IssueAnalysisRequestedConsumer> logger)
    : IConsumer<IssueAnalysisRequested>
{
    public async Task Consume(ConsumeContext<IssueAnalysisRequested> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing IssueAnalysisRequested for Issue {IssueId} (CorrelationId: {CorrelationId})",
            message.IssueId, message.CorrelationId);

        // 1. Invoke AI abstraction (Ollama or test mock)
        var analysis = await issueAnalyzer.AnalyzeIssueAsync(
            message.RawDescription,
            message.Location,
            context.CancellationToken);

        // 2. Deterministic priority calculation rule
        var severityPoints = analysis.Severity switch
        {
            "LOW" => 1,
            "MEDIUM" => 3,
            "HIGH" => 5,
            "CRITICAL" => 6,
            _ => 2
        };

        var riskPoints = analysis.RiskLevel switch
        {
            "LOW" => 1,
            "MEDIUM" => 2,
            "HIGH" => 3,
            "HAZARDOUS" => 4,
            _ => 1
        };

        var calculatedPriority = Math.Clamp(severityPoints + riskPoints, 1, 10);

        // 3. Publish IssueAnalyzed integration event
        await context.Publish(new IssueAnalyzed(
            message.IssueId,
            analysis.Category,
            analysis.RequiredSkills,
            analysis.Severity,
            analysis.RiskLevel,
            calculatedPriority,
            analysis.Tags,
            analysis.Confidence,
            Guid.NewGuid(),
            DateTime.UtcNow,
            message.CorrelationId
        ), context.CancellationToken);

        logger.LogInformation("Completed AI analysis for Issue {IssueId}. Assigned category {Category} and Priority {Priority}",
            message.IssueId, analysis.Category, calculatedPriority);
    }
}
