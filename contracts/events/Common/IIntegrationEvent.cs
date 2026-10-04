namespace FixFlow.Contracts.Common;

/// <summary>
/// Base interface for all integration events published across FixFlow message bus.
/// Guarantees traceability, ordering diagnostics, and schema version evolution.
/// </summary>
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTime OccurredAtUtc { get; }
    string CorrelationId { get; }
    int SchemaVersion { get; }
}
