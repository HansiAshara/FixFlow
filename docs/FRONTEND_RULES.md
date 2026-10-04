# FixFlow - Frontend Engineering Rules

## 1. Directory & Feature Organization
- All features live in `apps/web/src/features/<feature-name>/`.
- Standard sub-directories per feature:
  - `api/` (TanStack Query hooks, query keys, API caller functions)
  - `components/` (Feature-specific presentation and UI components)
  - `pages/` (Thin routing views composing feature components)
  - `schemas/` (Zod validation schemas)
  - `types/` (TypeScript interfaces and types)

## 2. State Management Rules
- **Server State**: Managed strictly with **TanStack Query** (`useQuery`, `useMutation`).
- **Do NOT copy query results into local `useState`**: Consume query data directly from `data`.
- **Form State**: Managed with **React Hook Form** coupled with **Zod** resolvers.

## 3. UI/UX Invariants
- Every asynchronous component MUST provide explicit **Loading**, **Error**, and **Empty** states.
- Reusable UI primitives live in `src/shared/components/`.
- Use Tailwind CSS design tokens defined in `src/styles/index.css`.
- Fully responsive across mobile, tablet, and desktop breakpoints.
