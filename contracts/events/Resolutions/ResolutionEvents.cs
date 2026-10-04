using FixFlow.Contracts.Common;

namespace FixFlow.Contracts.Events.Resolutions;

public record ResolutionVerified(
    Guid ResolutionId,
    Guid WorkOrderId,
    Guid ReporterId,
    bool IsSatisfied,
    int Rating,
    string? Feedback,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

public record ResolutionReopened(
    Guid ResolutionId,
    Guid WorkOrderId,
    Guid ReporterId,
    string ReopenReason,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;
