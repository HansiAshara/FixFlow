namespace FixFlow.Worker.Abstractions;

public record AiIssueAnalysisResult(
    string Category,
    IReadOnlyList<string> RequiredSkills,
    string Severity,
    string RiskLevel,
    IReadOnlyList<string> Tags,
    double Confidence
);

public record AiResolutionAssistantResult(
    IReadOnlyList<string> SuggestedActionItems,
    IReadOnlyList<string> PreventionTips,
    double SummaryQualityScore
);

/// <summary>
/// Provider-independent AI abstraction for natural language issue parsing.
/// </summary>
public interface IAiIssueAnalyzer
{
    Task<AiIssueAnalysisResult> AnalyzeIssueAsync(
        string rawDescription,
        string location,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Provider-independent AI abstraction for post-resolution assistance and prevention insights.
/// </summary>
public interface IAiResolutionAssistant
{
    Task<AiResolutionAssistantResult> AssistResolutionAsync(
        string issueDescription,
        string technicianWorkSummary,
        CancellationToken cancellationToken = default);
}
