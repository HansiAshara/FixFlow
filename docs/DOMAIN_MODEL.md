# FixFlow - Domain Model

## 1. Domain Aggregates & Boundaries

### 1. Identity Module
- **ApplicationUser**: Root user identity (Email, PasswordHash, FullName, Status).
- **ApplicationRole**: User roles (`USER`, `TECHNICIAN`, `ADMIN`).

### 2. Issues Module
- **Issue (Aggregate Root)**: Represents a user problem report.
  - Value Objects: `IssueSeverity` (`LOW`, `MEDIUM`, `HIGH`, `CRITICAL`), `IssueRiskLevel` (`LOW`, `MEDIUM`, `HIGH`, `HAZARDOUS`).
  - Fields: `RawDescription`, `Location`, `Category`, `RequiredSkills`, `CalculatedPriority`, `Status`.

### 3. Technicians Module
- **TechnicianProfile (Aggregate Root)**: Professional bio, experience years, skills, verification status (`Unverified`, `PendingReview`, `Probationary`, `Verified`, `Suspended`).
- **TechnicianEvidence (Entity)**: Trade license, formal certificate, or experience portfolio verified by an admin.

### 4. WorkOrders Module
- **WorkOrder (Aggregate Root)**: Lifecycle contract between reporter and technician.
  - Fields: `IssueId`, `ReporterId`, `AssignedTechnicianId`, `Status`, `AwaitingResponseUntilUtc`, `StartedAtUtc`, `ClosedAtUtc`.

### 5. Resolutions Module
- **Resolution (Aggregate Root)**: Records satisfaction, rating (1–5), customer review, or reopen reason.

### 6. Performance Module
- **TechnicianPerformance (Aggregate Root)**: Aggregates historical metrics (HistoricalScore, ResponseRate, CompletionRate, AverageRating).
