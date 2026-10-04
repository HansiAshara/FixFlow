# FixFlow - Product Requirements Document (PRD)

## 1. Problem Statement
Homeowners and business property managers facing maintenance, electrical, plumbing, or structural problems frequently struggle with:
1. **Diagnosis Uncertainty**: Not knowing which trade or specific qualification is required.
2. **Technician Trust & Suitability**: Difficulties identifying dependable, verified technicians with proven track records.
3. **Opaque Resolution Lifecycle**: Lack of clear visibility into work status, response times, and post-resolution guarantees.

## 2. Solution Overview
FixFlow is an intelligent issue-reporting and technician-matching platform where users submit problems in natural language. FixFlow executes:
1. **Asynchronous AI Issue Analysis**: Extracts categories, required skills, severity, risk level, and tags.
2. **Deterministic Priority Calculation**: Merges severity and hazard risk into a 1–10 priority scale.
3. **Multi-Factor Technician Ranking**: Combines skill coverage, verified experience, reliability history, and real-time response performance.
4. **Authoritative Work Order State Machine**: Manages the end-to-end lifecycle with explicit state transitions.
5. **Human-in-the-Loop Verification**: Requires administrator review for formal certifications and trade licenses.
6. **Performance Feedback Loop**: Real job completion rates, response velocity, and customer verifications continuously train future recommendations.

## 3. User Roles & Capabilities
- **Reporter (User)**: Reports issues, reviews ranked technicians, creates work orders, tracks status, verifies or reopens completed jobs.
- **Technician**: Creates profile, submits verification evidence, receives assigned work orders, accepts/rejects, updates progress, submits completion notes.
- **Administrator**: Reviews qualification evidence, audits system events, monitors platform health.
