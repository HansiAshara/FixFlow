# FixFlow - Event Contracts & Distributed Architecture

FixFlow uses MassTransit with RabbitMQ as its integration event bus.

---

## 1. Integration Event Envelope
All integration events implement `IIntegrationEvent`:
- `EventId (Guid)`: Unique identifier for idempotency tracking.
- `OccurredAtUtc (DateTime)`: Event creation timestamp.
- `CorrelationId (string)`: End-to-end request tracing key.
- `SchemaVersion (int)`: Version tracking for contract evolution.

---

## 2. Event Catalog

### `IssueAnalysisRequested`
- **Producer**: `FixFlow.Api` (`CreateIssueHandler`)
- **Consumer**: `FixFlow.Worker` (`IssueAnalysisRequestedConsumer`)
- **Payload**: `IssueId`, `ReporterId`, `RawDescription`, `Location`

### `IssueAnalyzed`
- **Producer**: `FixFlow.Worker` (`IssueAnalysisRequestedConsumer`)
- **Consumer**: `FixFlow.Api` (Issue state update & Matching notification)
- **Payload**: `IssueId`, `Category`, `RequiredSkills`, `Severity`, `RiskLevel`, `CalculatedPriority`, `ExtractedTags`, `AiConfidenceScore`

### `WorkOrderCreated`
- **Producer**: `FixFlow.Api`
- **Payload**: `WorkOrderId`, `IssueId`, `ReporterId`, `AssignedTechnicianId`, `AwaitingResponseUntilUtc`

### `WorkOrderAccepted` / `WorkOrderRejected` / `WorkOrderExpired`
- **Producer**: `FixFlow.Api` / Background Scheduler

### `WorkStarted` / `WorkCompleted`
- **Producer**: `FixFlow.Api`

### `ResolutionVerified` / `ResolutionReopened`
- **Producer**: `FixFlow.Api`

### `PerformanceUpdated`
- **Producer**: `FixFlow.Worker` (Performance aggregator consumer)
- **Payload**: `TechnicianId`, `HistoricalScore`, `ResponseRate`, `CompletionRate`, `TotalJobsCompleted`

---

## 3. Resilience & Dead-Letter Strategy
1. **Exponential Retry**: Failed consumers automatically retry 3 times with exponential backoff (1s, 2s, 4s).
2. **Error Queues**: Messages exceeding retry limits move to `_error` queues (e.g. `issue-analysis-requested_error`) for developer inspection without blocking other messages.
3. **Skipped Queues**: Unhandled message types move to `_skipped` queues.
