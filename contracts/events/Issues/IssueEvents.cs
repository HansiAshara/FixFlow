using FixFlow.Contracts.Common;

namespace FixFlow.Contracts.Events.Issues;

/// <summary>
/// Published when a user reports an issue in natural language and requests asynchronous AI analysis.
/// </summary>
public record IssueAnalysisRequested(
    Guid IssueId,
    Guid ReporterId,
    string RawDescription,
    string Location,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

/// <summary>
/// Published by the AI worker after Ollama parses and validates structured features from the issue.
/// </summary>
public record IssueAnalyzed(
    Guid IssueId,
    string Category,
    IReadOnlyList<string> RequiredSkills,
    string Severity,
    string RiskLevel,
    int CalculatedPriority,
    IReadOnlyList<string> ExtractedTags,
    double AiConfidenceScore,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;
