# Contributing to FixFlow

Thank you for contributing to FixFlow. This guide outlines the development standards, git branching model, and code review process.

---

## 1. Team Ownership Matrix

| Feature / Domain Module | Primary Contributor | Backup Reviewer |
| :--- | :--- | :--- |
| **Issues Module** (`apps/web/features/issues`, `api/Modules/Issues`) | Contributor 1 | Contributor 2 |
| **Matching Engine** (`apps/web/features/matching`, `api/Modules/Matching`) | Contributor 1 | Contributor 2 |
| **Technicians & Evidence** (`apps/web/features/technicians`, `api/Modules/Technicians`) | Contributor 2 | Contributor 1 |
| **Work Orders & State Machine** (`apps/web/features/work-orders`, `api/Modules/WorkOrders`) | Contributor 2 | Contributor 1 |
| **Resolutions & Feedback** (`apps/web/features/resolutions`, `api/Modules/Resolutions`) | Contributor 2 | Contributor 1 |
| **Technician Performance** (`apps/web/features/performance`, `api/Modules/Performance`) | Contributor 2 | Contributor 1 |
| **Identity & Authentication** | Shared | Shared |
| **Notifications & Administration** | Shared | Shared |
| **Contracts & Infrastructure (Docker/CI)** | Shared | Shared |

---

## 2. Git Workflow & Branching Strategy

- `main`: Production-ready, stable releases.
- `develop`: Integration branch for active development.
- `feat/<module>-<description>`: New features (branched from `develop`).
- `fix/<module>-<description>`: Bug fixes (branched from `develop`).
- `docs/<description>`: Documentation changes.
- `chore/<description>`: Tooling, dependency updates, CI workflows.

**Rules**:
- Never commit directly to `main` or `develop`.
- Always open a Pull Request targeting `develop`.

---

## 3. Conventional Commit Messages

Follow the Conventional Commits specification:

```
<type>(<scope>): <short summary>

[optional body]
```

**Types**:
- `feat`: New user-facing feature
- `fix`: Bug fix
- `docs`: Documentation updates
- `test`: Adding or modifying automated tests
- `refactor`: Code changes that neither fix a bug nor add a feature
- `chore`: CI, tooling, or dependency updates

**Examples**:
- `feat(issues): add natural language issue reporting endpoint`
- `fix(workorders): prevent invalid state transition from completed to in-progress`
- `test(matching): add unit tests for deterministic score weighting`
- `docs(rbac): document work order state authorization policies`

---

## 4. Pull Request Checklist

Before submitting a PR for review:
1. `dotnet test FixFlow.sln` passes with 0 failures.
2. `cd apps/web && npm run typecheck && npm run test && npm run build` passes.
3. No secrets or credentials in code or commits.
4. Any new API endpoints or event contracts documented in `docs/`.
5. Code adheres to `.editorconfig` formatting rules.
