# FixFlow - Technician Matching Algorithm Specification

## 1. Matching Objectives
When an issue is analyzed, FixFlow ranks eligible technicians based on multi-dimensional scoring rather than simple proximity or lowest bid.

---

## 2. Mathematical Formula

$$OverallScore = (S \times 0.40) + (E \times 0.25) + (H \times 0.20) + (A \times 0.15)$$

Where:
- **$S$ (Skill Match Score - 40%)**:
  $$S = \frac{|RequiredSkills \cap TechnicianSkills|}{|RequiredSkills|}$$
- **$E$ (Experience & Verification Weight - 25%)**:
  - `VERIFIED` + $\ge 5$ yrs experience = $1.0$
  - `VERIFIED` + $< 5$ yrs experience = $0.85$
  - `PROBATIONARY` = $0.65$
  - `PENDING_REVIEW` / `UNVERIFIED` = $0.0$ (Ineligible)
- **$H$ (Historical FixFlow Performance - 20%)**:
  - Normalized rating and completed job reliability score ($0.0 \dots 1.0$).
- **$A$ (Availability Score - 15%)**:
  - $1.0$ if currently marked available with no active concurrent jobs; $0.5$ if active; $0.0$ if unavailable.

---

## 3. Tie Breaking & Safety Constraints
1. Technicians with `Suspended` or `Rejected` verification statuses are excluded immediately.
2. If two technicians share identical scores, preference is given to the technician with higher historical customer verification ratings.
