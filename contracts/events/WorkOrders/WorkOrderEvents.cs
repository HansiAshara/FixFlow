using FixFlow.Contracts.Common;

namespace FixFlow.Contracts.Events.WorkOrders;

public record WorkOrderCreated(
    Guid WorkOrderId,
    Guid IssueId,
    Guid ReporterId,
    Guid AssignedTechnicianId,
    DateTime AwaitingResponseUntilUtc,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

public record WorkOrderAccepted(
    Guid WorkOrderId,
    Guid AssignedTechnicianId,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

public record WorkOrderRejected(
    Guid WorkOrderId,
    Guid AssignedTechnicianId,
    string Reason,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

public record WorkOrderExpired(
    Guid WorkOrderId,
    Guid AssignedTechnicianId,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

public record WorkStarted(
    Guid WorkOrderId,
    Guid AssignedTechnicianId,
    DateTime StartedAtUtc,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;

public record WorkCompleted(
    Guid WorkOrderId,
    Guid AssignedTechnicianId,
    string TechnicianSummary,
    DateTime CompletedAtUtc,
    Guid EventId,
    DateTime OccurredAtUtc,
    string CorrelationId,
    int SchemaVersion = 1
) : IIntegrationEvent;
