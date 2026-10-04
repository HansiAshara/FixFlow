# FixFlow - Git Workflow & Branching Guidelines

## 1. Branch Strategy
- `main`: Production releases only. Protected branch.
- `develop`: Shared integration branch for active features.
- Feature branches: `feat/<module>-<description>` (e.g., `feat/issues-reporting`).
- Bugfix branches: `fix/<module>-<description>` (e.g., `fix/workorder-transition`).
- Documentation branches: `docs/<description>`.
- Chores: `chore/<description>`.

---

## 2. Commit Message Convention
Follow Conventional Commits:
```
<type>(<scope>): <description>
```
Examples:
- `feat(issues): implement natural language issue creation use case`
- `fix(workorders): enforce valid transition from in_progress to awaiting_reporter`
- `test(performance): add unit test for penalty calculation`
- `chore(deps): update npm packages to latest minor versions`
