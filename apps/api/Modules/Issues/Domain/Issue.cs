using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Issues.Domain;

public enum IssueStatus
{
    Draft = 0,
    Reported = 1,
    AnalysisRequested = 2,
    Analyzed = 3,
    MatchingInProgress = 4,
    TechnicianAssigned = 5,
    Resolved = 6,
    Closed = 7,
    Cancelled = 8
}

public enum IssueSeverity
{
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}

public enum IssueRiskLevel
{
    Low = 1,
    Medium = 2,
    High = 3,
    Hazardous = 4
}

public class Issue : Entity<Guid>, IAggregateRoot
{
    public Guid ReporterId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string RawDescription { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;
    public IssueStatus Status { get; private set; }
    public string? Category { get; private set; }
    public List<string> RequiredSkills { get; private set; } = [];
    public IssueSeverity Severity { get; private set; } = IssueSeverity.Medium;
    public IssueRiskLevel RiskLevel { get; private set; } = IssueRiskLevel.Low;
    public int CalculatedPriority { get; private set; }
    public List<string> Tags { get; private set; } = [];
    public double? AiConfidenceScore { get; private set; }

    private Issue() { }

    public Issue(Guid id, Guid reporterId, string title, string rawDescription, string location) : base(id)
    {
        ReporterId = reporterId;
        Title = title;
        RawDescription = rawDescription;
        Location = location;
        Status = IssueStatus.Reported;
        CalculatedPriority = IssuePriorityCalculator.Calculate(Severity, RiskLevel);
    }

    public void MarkAnalysisRequested()
    {
        Status = IssueStatus.AnalysisRequested;
        MarkUpdated();
    }

    public void ApplyAnalysisResults(
        string category,
        IEnumerable<string> requiredSkills,
        IssueSeverity severity,
        IssueRiskLevel riskLevel,
        IEnumerable<string> tags,
        double confidenceScore)
    {
        Category = category;
        RequiredSkills = requiredSkills.ToList();
        Severity = severity;
        RiskLevel = riskLevel;
        Tags = tags.ToList();
        AiConfidenceScore = confidenceScore;
        CalculatedPriority = IssuePriorityCalculator.Calculate(severity, riskLevel);
        Status = IssueStatus.Analyzed;
        MarkUpdated();
    }

    public void AssignTechnician()
    {
        Status = IssueStatus.TechnicianAssigned;
        MarkUpdated();
    }
}

/// <summary>
/// Deterministic issue priority calculation combining severity and risk levels.
/// Priority ranges from 1 (lowest) to 10 (emergency/critical).
/// </summary>
public static class IssuePriorityCalculator
{
    public static int Calculate(IssueSeverity severity, IssueRiskLevel riskLevel)
    {
        // Base weight: Severity contributes up to 6 points, Risk level up to 4 points.
        var severityPoints = severity switch
        {
            IssueSeverity.Low => 1,
            IssueSeverity.Medium => 3,
            IssueSeverity.High => 5,
            IssueSeverity.Critical => 6,
            _ => 2
        };

        var riskPoints = riskLevel switch
        {
            IssueRiskLevel.Low => 1,
            IssueRiskLevel.Medium => 2,
            IssueRiskLevel.High => 3,
            IssueRiskLevel.Hazardous => 4,
            _ => 1
        };

        return Math.Clamp(severityPoints + riskPoints, 1, 10);
    }
}
