# FixFlow - Work Order State Machine Specification

## 1. Approved State Diagram

```mermaid
stateDiagram-v2
    [*] --> AWAITING_RESPONSE
    AWAITING_RESPONSE --> ACCEPTED: Technician Accepts
    AWAITING_RESPONSE --> REJECTED: Technician Rejects
    AWAITING_RESPONSE --> EXPIRED: SLA Timeout (24h)
    AWAITING_RESPONSE --> CANCELLED: Reporter Cancels

    ACCEPTED --> READY_TO_START
    ACCEPTED --> IN_PROGRESS: Work Started
    ACCEPTED --> CANCELLED: Reporter Cancels

    READY_TO_START --> IN_PROGRESS: Technician Arrives / Starts
    READY_TO_START --> CANCELLED: Reporter Cancels

    IN_PROGRESS --> TECHNICIAN_COMPLETED: Technician Submits Work
    IN_PROGRESS --> AWAITING_REPORTER: Direct Completion Transition

    TECHNICIAN_COMPLETED --> AWAITING_REPORTER

    AWAITING_REPORTER --> COMPLETED: Reporter Verifies Resolution
    AWAITING_REPORTER --> REOPENED: Reporter Reopens Issue

    REOPENED --> IN_PROGRESS: Rework Commenced
    REOPENED --> CANCELLED: Terminated

    COMPLETED --> [*]
    REJECTED --> [*]
    EXPIRED --> [*]
    CANCELLED --> [*]
```

## 2. Transition Authority Matrix

| Current State | Target State | Authorized Actor | Pre-Conditions |
| :--- | :--- | :--- | :--- |
| `AWAITING_RESPONSE` | `ACCEPTED` | Assigned Technician | Before `AwaitingResponseUntilUtc` |
| `AWAITING_RESPONSE` | `REJECTED` | Assigned Technician | Valid rejection reason provided |
| `AWAITING_RESPONSE` | `EXPIRED` | System Worker | `UtcNow > AwaitingResponseUntilUtc` |
| `AWAITING_RESPONSE` | `CANCELLED` | Issue Reporter | Before acceptance |
| `ACCEPTED` | `IN_PROGRESS` | Assigned Technician | On-site arrival |
| `IN_PROGRESS` | `AWAITING_REPORTER`| Assigned Technician | Summary & completion notes provided |
| `AWAITING_REPORTER` | `COMPLETED` | Issue Reporter | Verification rating provided (1–5) |
| `AWAITING_REPORTER` | `REOPENED` | Issue Reporter | Specific rework reason provided |

All transitions are strictly validated in `WorkOrderStateMachine.cs`. Any attempt to trigger an invalid transition throws a `ConflictException` translated to an RFC 7807 HTTP 409 response.
