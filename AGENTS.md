# FixFlow AI Agent & Developer Rules

This document defines the strict architectural boundaries, security policies, and engineering standards for both human contributors and AI coding agents working on FixFlow.

---

## 1. Prime Directive
- **Read before implementing**: Always consult relevant documentation in `docs/` before writing code for any module.
- **Do not bypass boundaries**: Preserve modular monolith boundaries. No circular module dependencies or cross-module database mutations.
- **Never hardcode secrets**: All configuration must come from environment variables or `appsettings.json`.

---

## 2. Contributor Ownership Boundaries
- **Contributor 1 (Issue & Matching Intelligence)**:
  - `apps/web/src/features/issues/`
  - `apps/web/src/features/matching/`
  - `apps/api/Modules/Issues/`
  - `apps/api/Modules/Matching/`
  - AI Issue Analysis background consumers (`apps/worker/Consumers/IssueAnalysisRequestedConsumer.cs`)
- **Contributor 2 (Technician & Work Intelligence)**:
  - `apps/web/src/features/technicians/`
  - `apps/web/src/features/work-orders/`
  - `apps/web/src/features/resolutions/`
  - `apps/web/src/features/performance/`
  - `apps/api/Modules/Technicians/`
  - `apps/api/Modules/WorkOrders/`
  - `apps/api/Modules/Resolutions/`
  - `apps/api/Modules/Performance/`
  - Workflow & Performance background consumers
- **Shared Modules & Infrastructure**:
  - `Identity`, `Notifications`, `Administration`, `contracts/`, `infra/`, `FixFlowDbContext.cs`, CI workflows. Changes to shared components require mutual review.

---

## 3. Trust Boundaries & Security Rules
- **Frontend input is UNTRUSTED**: All HTTP requests must undergo server-side validation (FluentValidation) and server-side authorization.
- **AI output is UNTRUSTED**: LLM output from Ollama must be strictly validated against JSON schemas, normalized, and clamped. AI is NEVER permitted to:
  - Autonomously verify technician credentials or certificates.
  - Make authorization or permission decisions.
  - Directly mutate WorkOrder state.
- **RabbitMQ messages are UNTRUSTED**: Message consumers must validate message contents and implement idempotent handling.
- **Frontend guards are UX-only**: `AuthGuard` and `RoleGuard` exist for routing/UI presentation only. The ASP.NET Core API is the sole authoritative security boundary.

---

## 4. Backend Rules (.NET 9 / C# 13)
- Use vertical slices (`Domain/`, `Application/`, `Endpoints/`) within each module.
- Endpoints must remain thin; business logic belongs in Domain aggregates and Application use cases.
- Use explicit C# nullable reference types.
- Always pass `CancellationToken` through asynchronous I/O methods.
- Store all timestamps in UTC (`DateTime.UtcNow`).
- Use the `Result` pattern for predictable business logic errors; use custom exceptions (`NotFoundException`, `ForbiddenException`, `ValidationException`) for RFC 7807 `ProblemDetails` translation.
- Never write ad-hoc SQL or bypass EF Core migrations.

---

## 5. Frontend Rules (React 19 / TypeScript / Vite)
- Feature-based organization: Code specific to a feature stays inside `src/features/<feature-name>/`.
- `shared/` contains only genuinely generic and reusable utilities and components.
- Server state MUST be managed with **TanStack Query**. Never duplicate query results into local `useState`.
- Forms MUST use **React Hook Form** + **Zod** schemas.
- Pages compose feature components; keep page components lightweight.
- Mandatory UI states: Every asynchronous view MUST handle **Loading**, **Error**, and **Empty** states.
- Strict TypeScript: No `any`.

---

## 6. Definition of Done (DoD) for Any Feature
Every Pull Request / feature branch must satisfy:
1. Complete frontend components with responsive styling.
2. Complete backend vertical use case with validation and authorization.
3. Automated unit tests covering domain rules and edge cases.
4. Architecture tests passing (`dotnet test tests/FixFlow.ArchitectureTests`).
5. Manual desktop and mobile verification in browser.
6. Documentation updated if APIs or event contracts changed.
