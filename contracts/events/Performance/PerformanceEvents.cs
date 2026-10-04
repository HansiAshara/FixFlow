using FixFlow.Contracts.Common;

namespace FixFlow.Contracts.Events.Performance;

public record PerformanceUpdated(
    Guid TechnicianId,
    double HistoricalScore,
    double ResponseRate,
    double CompletionRate,
    int TotalJobsCompleted,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;
