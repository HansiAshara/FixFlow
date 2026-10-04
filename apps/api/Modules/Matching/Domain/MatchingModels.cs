using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Matching.Domain;

public record TechnicianMatchCriteria(
    Guid IssueId,
    string Category,
    IReadOnlyList<string> RequiredSkills,
    int Priority,
    string Location
);

public record RankedTechnician(
    Guid TechnicianId,
    string FullName,
    double OverallScore,
    double SkillMatchScore,
    double ExperienceScore,
    double AvailabilityScore,
    double ReliabilityScore,
    double HistoricalPerformanceScore,
    string VerificationStatus
);

public static class MatchingAlgorithm
{
    /// <summary>
    /// Deterministic matching calculation combining skill overlap, verified experience,
    /// historical FixFlow ratings, and response performance.
    /// Weights: Skill Match (40%), Experience & Verification (25%), Historical Performance (20%), Availability (15%).
    /// </summary>
    public static double CalculateScore(
        double skillMatchRate,
        double experienceWeight,
        double historicalPerformance,
        double availabilityScore)
    {
        var score = (skillMatchRate * 0.40) +
                    (experienceWeight * 0.25) +
                    (historicalPerformance * 0.20) +
                    (availabilityScore * 0.15);

        return Math.Round(Math.Clamp(score, 0.0, 1.0), 3);
    }
}
