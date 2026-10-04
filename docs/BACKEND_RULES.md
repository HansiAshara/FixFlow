# FixFlow - Backend Engineering Rules

## 1. Modular Monolith & Vertical Slices
- Do NOT create global God controllers or monolithic service layers.
- Modules in `apps/api/Modules/` contain:
  - `Domain/` (Aggregates, Entities, Value Objects, Domain Rules)
  - `Application/` (Use cases organized by command/query with validation and handlers)
  - `Endpoints/` (Thin Minimal API routes mapping to application handlers)
  - `Infrastructure/` (EF Core mappings and external integrations)

## 2. Dependency Direction
$$\text{Domain} \leftarrow \text{Application} \leftarrow \text{Infrastructure} \leftarrow \text{API / Endpoints}$$
- Domain has 0 external dependencies.
- Application depends only on Domain.
- Infrastructure and API depend on Application and Domain abstractions.
- These boundaries are validated automatically by `FixFlow.ArchitectureTests`.

## 3. Error Handling & RFC 7807 ProblemDetails
- Use the `Result` pattern for domain validation failures.
- Throw custom `AppException` subclasses (`NotFoundException`, `ValidationException`, `ForbiddenException`, `ConflictException`) for HTTP error translation via `GlobalExceptionHandler`.
- Never expose raw exception stack traces in production responses.
