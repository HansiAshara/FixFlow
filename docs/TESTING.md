# FixFlow - Testing Strategy & Quality Assurance

## 1. Test Projects Structure

```
tests/
├── FixFlow.UnitTests/          # Pure domain & algorithm unit tests (xUnit + FluentAssertions)
├── FixFlow.IntegrationTests/   # WebApplicationFactory integration tests with real HTTP routes
└── FixFlow.ArchitectureTests/  # NetArchTest layer boundary & dependency enforcement tests
```

## 2. Test Execution Commands

```bash
# Run all backend test suites
dotnet test FixFlow.sln

# Run frontend tests
cd apps/web && npm run test

# Run frontend typecheck
cd apps/web && npm run typecheck
```

## 3. Definition of Done (DoD)
A feature branch is ready for merge only when:
- 100% of automated unit, integration, and architecture tests pass.
- Frontend typecheck (`npm run typecheck`) passes with zero errors.
- Manual responsive testing has been verified on both mobile and desktop viewports.
- All loading, error, and empty UI states are verified.
