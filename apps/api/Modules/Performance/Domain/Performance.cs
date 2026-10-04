using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Performance.Domain;

public class TechnicianPerformance : Entity<Guid>, IAggregateRoot
{
    public Guid TechnicianId { get; private set; }
    public double HistoricalScore { get; private set; }
    public double ResponseRate { get; private set; }
    public double CompletionRate { get; private set; }
    public int TotalAssignedJobs { get; private set; }
    public int TotalCompletedJobs { get; private set; }
    public int TotalReopenedJobs { get; private set; }
    public double AverageRating { get; private set; }

    private TechnicianPerformance() { }

    public TechnicianPerformance(Guid id, Guid technicianId) : base(id)
    {
        TechnicianId = technicianId;
        HistoricalScore = 0.5; // Default neutral baseline
        ResponseRate = 1.0;
        CompletionRate = 1.0;
        AverageRating = 5.0;
    }

    public void Recalculate(int assigned, int completed, int reopened, double avgRating, double responseRate)
    {
        TotalAssignedJobs = assigned;
        TotalCompletedJobs = completed;
        TotalReopenedJobs = reopened;
        AverageRating = avgRating;
        ResponseRate = Math.Clamp(responseRate, 0.0, 1.0);
        CompletionRate = assigned > 0 ? (double)completed / assigned : 1.0;

        HistoricalScore = PerformanceCalculator.CalculateOverallScore(CompletionRate, ResponseRate, AverageRating, reopened, assigned);
        MarkUpdated();
    }
}

public static class PerformanceCalculator
{
    /// <summary>
    /// Deterministic performance calculation:
    /// Weights: Rating (40%), Completion Rate (30%), Response Rate (20%), Reopen penalty (10%).
    /// </summary>
    public static double CalculateOverallScore(
        double completionRate,
        double responseRate,
        double avgRating,
        int reopenedCount,
        int totalAssigned)
    {
        var normalizedRating = Math.Clamp(avgRating / 5.0, 0.0, 1.0);
        var reopenPenalty = totalAssigned > 0 ? Math.Min(0.2, (double)reopenedCount / totalAssigned) : 0.0;

        var rawScore = (normalizedRating * 0.40) +
                       (completionRate * 0.30) +
                       (responseRate * 0.20) -
                       reopenPenalty + 0.10;

        return Math.Round(Math.Clamp(rawScore, 0.0, 1.0), 3);
    }
}
