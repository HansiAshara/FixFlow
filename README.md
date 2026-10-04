# FixFlow - Intelligent Issue Reporting & Technician Matching Platform

FixFlow is a production-oriented, modular platform that pairs natural language issue diagnosis with verified technician matching and authoritative work order lifecycle governance.

---

## 1. Project Overview
FixFlow simplifies maintenance and repair workflows for homeowners, facility managers, and service technicians by converting free-text problem descriptions into structured work orders and ranking eligible technicians based on verified skills and real-world performance history.

## 2. Real-World Problem
- Users facing maintenance or electrical problems frequently cannot diagnose the root cause or identify the precise trade qualification required.
- Finding dependable, verified technicians with proven reliability is difficult and prone to fake reviews.
- Lack of authoritative work order state tracking leads to unresolved disputes, missed SLAs, and communication breakdowns.

## 3. FixFlow Solution
- **Natural Language Reporting**: Users describe their issue without needing trade jargon.
- **Local AI Analysis**: Asynchronous background workers categorize problems, assess risks, and determine required skill tags.
- **Deterministic Prioritization & Matching**: Objective mathematical formulas score urgency and rank technicians.
- **Authoritative State Machine**: Backend-governed work order states enforce step-by-step progress from acceptance to customer verification.
- **Human-in-the-Loop Verification**: Administrators rigorously review trade licenses and diplomas before granting verified status.

## 4. Main User Workflow
1. User reports an issue in natural language.
2. System dispatches an asynchronous AI analysis event via RabbitMQ.
3. Worker invokes Ollama LLM to extract categories, skills, and risk levels.
4. Issue priority is calculated deterministically (1–10).
5. Matching engine ranks eligible, verified technicians.
6. User selects a technician, creating an authoritative work order.
7. Technician accepts or rejects within the SLA window.
8. Technician records progress and marks work completed.
9. User verifies the resolution with ratings/feedback or reopens if unsatisfactory.
10. Feedback updates the technician's permanent performance score.

## 5. Reporter Capabilities
- Report maintenance issues with natural language text and location.
- View AI classification tags and calculated priority.
- Browse and select ranked technicians.
- Track real-time work order status.
- Verify work resolution or trigger reopening.

## 6. Technician Capabilities
- Build a professional trade profile with experience and skill tags.
- Upload formal certificates, licenses, and experience portfolios.
- Accept or reject assigned work orders within the SLA window.
- Transition work status (`IN_PROGRESS`, `AWAITING_REPORTER`).
- View personal performance metrics (ratings, response rates, completion rates).

## 7. Admin Responsibilities
- Review and verify uploaded technician qualification documents (strict human-in-the-loop governance).
- Manage user account statuses and suspensions.
- Inspect system-wide audit logs and health probes.

## 8. AI Capabilities
- Natural language parsing using local LLMs (Ollama `llama3.2:3b`).
- Category classification (`Plumbing`, `Electrical`, `HVAC`, `Carpentry`, etc.).
- Skill extraction and risk assessment.
- Post-resolution insights and prevention tips.
*Note: AI output is strictly treated as untrusted input and validated against JSON schemas.*

## 9. Matching Approach
Technician recommendation scores combine four weighted factors:
- **Skill Match (40%)**: Overlap between required skills and technician verified skills.
- **Experience & Verification (25%)**: Years of experience and admin verification tier.
- **Historical Performance (20%)**: FixFlow job completion history and verified reviews.
- **Availability (15%)**: Real-time capacity and active workload.

## 10. Technician Performance Feedback Loop
Every verified resolution, response time, and customer rating updates the technician's historical scorecard. Reopened work orders apply a mathematical penalty, ensuring recommendation algorithms favor consistent, high-quality providers.

## 11. Distributed Architecture
A **Modular Monolith** backend (`FixFlow.Api`) handles synchronous client HTTP requests, while distributed background workers (`FixFlow.Worker`) consume messages from **RabbitMQ** via **MassTransit** to run asynchronous AI classification and performance recalculations.

## 12. Technology Stack
- **Frontend**: React 19, TypeScript, Vite, Tailwind CSS, TanStack Query, React Hook Form, Zod, Vitest.
- **Backend**: .NET 9, C# 13, ASP.NET Core, EF Core 9, ASP.NET Core Identity, FluentValidation, MassTransit.
- **Database**: PostgreSQL 16.
- **Message Broker**: RabbitMQ 3.13.
- **AI Engine**: Ollama (local LLM inference).
- **Infrastructure**: Docker, Docker Compose, GitHub Actions.

## 13. Repository Structure
```
FixFlow/
├── apps/
│   ├── web/                    # React + TypeScript + Tailwind frontend
│   ├── api/                    # ASP.NET Core Modular Monolith API
│   └── worker/                 # .NET Worker Service (MassTransit + Ollama)
├── contracts/
│   ├── events/                 # C# MassTransit Integration Event Contracts
│   └── schemas/                # JSON Schemas for AI structured validation
├── tests/
│   ├── FixFlow.UnitTests/      # Domain rule & algorithm unit tests
│   ├── FixFlow.IntegrationTests/# WebApplicationFactory API health tests
│   └── FixFlow.ArchitectureTests/# NetArchTest layer boundary enforcement
├── docs/                       # Comprehensive architectural & domain specifications
├── infra/
│   ├── docker/                 # Dockerfiles & Nginx configurations
│   └── scripts/                # Helper setup & model download scripts
├── .github/workflows/          # GitHub Actions CI workflow
├── docker-compose.yml          # Container orchestration definition
├── Directory.Build.props       # Centralized .NET 9 configuration
├── FixFlow.sln                 # .NET Solution file
├── AGENTS.md                   # Strict guidelines for AI agents & contributors
├── CONTRIBUTING.md             # Contributor ownership matrix & git rules
└── README.md
```

## 14. Domain Modules
1. `Identity`: User accounts, JWT token generation, role management.
2. `Issues`: Natural language issue reporting and priority determination.
3. `Matching`: Multi-factor technician ranking engine.
4. `Technicians`: Profiles, trade evidence, and verification statuses.
5. `WorkOrders`: Authoritative state machine for work order lifecycles.
6. `Resolutions`: Customer verification, rating, and rework requests.
7. `Performance`: Historical scorecards and performance feedback calculations.
8. `Notifications`: In-app and event-driven user notifications.
9. `Administration`: Human-in-the-loop verification and audit logs.

## 15. Authentication & RBAC Overview
- **Authentication**: Stateless JWT bearer tokens signed with HMAC-SHA256.
- **Roles**: `USER`, `TECHNICIAN`, `ADMIN`.
- **Authorization**: Enforces RBAC + Resource Ownership + Business State on the backend.

## 16. Security Approach
- Server-side validation via FluentValidation.
- Centralized RFC 7807 `ProblemDetails` error handling.
- Secure password hashing with PBKDF2.
- MIME allowlists and server-generated GUID filenames for file uploads.

## 17. Local Setup
### Prerequisites
- .NET 9.0 SDK
- Node.js (v20+ or v24) & npm
- Docker Desktop

```bash
# 1. Clone repository and copy environment settings
git clone https://github.com/your-org/FixFlow.git
cd FixFlow
cp .env.example .env
```

## 18. Environment Variables
See [.env.example](file:///d:/FixFlow/.env.example) for the full list of database, RabbitMQ, JWT, and Ollama settings.

## 19. Docker Usage
```bash
# Start all containers in background
docker compose up -d

# Pull the default AI model into Ollama container
powershell infra/scripts/pull-ollama-model.ps1
```

## 20. Database Migrations
```bash
# Apply EF Core migrations locally
dotnet ef database update --project apps/api/FixFlow.Api.csproj
```

## 21. Running Web / API / Worker Locally
```bash
# Run Backend API (Port 5000)
dotnet run --project apps/api/FixFlow.Api.csproj

# Run Background Worker
dotnet run --project apps/worker/FixFlow.Worker.csproj

# Run Frontend Web (Port 5173 / 3000)
cd apps/web
npm run dev
```

## 22. Testing
```bash
# Run all backend unit, integration, and architecture tests
dotnet test FixFlow.sln

# Run frontend tests
cd apps/web && npm run test

# Run frontend typecheck
cd apps/web && npm run typecheck
```

## 23. Git Workflow
- `main`: Release-ready code.
- `develop`: Integration branch.
- Feature branches: `feat/<module>-<description>`.
- Commit conventions: [Conventional Commits](https://www.conventionalcommits.org/).

## 24. Contributor Workflow & Ownership
- **Contributor 1**: Primary on `Issues`, `Matching`, AI Issue Analysis.
- **Contributor 2**: Primary on `Technicians`, `WorkOrders`, `Resolutions`, `Performance`.
- **Shared**: `Identity`, `Administration`, `Contracts`, Infrastructure.
See [CONTRIBUTING.md](file:///d:/FixFlow/CONTRIBUTING.md) for details.

## 25. Documentation Links
- [Product Requirements](file:///d:/FixFlow/docs/PRODUCT_REQUIREMENTS.md)
- [Architecture Specification](file:///d:/FixFlow/docs/ARCHITECTURE.md)
- [Domain Model](file:///d:/FixFlow/docs/DOMAIN_MODEL.md)
- [Database Design](file:///d:/FixFlow/docs/DATABASE.md)
- [API Contracts](file:///d:/FixFlow/docs/API_CONTRACTS.md)
- [Event Contracts](file:///d:/FixFlow/docs/EVENT_CONTRACTS.md)
- [AI Architecture & Trust Boundaries](file:///d:/FixFlow/docs/AI_DESIGN.md)
- [Technician Matching Algorithm](file:///d:/FixFlow/docs/MATCHING_ALGORITHM.md)
- [Technician Performance Algorithm](file:///d:/FixFlow/docs/PERFORMANCE_ALGORITHM.md)
- [Work Order State Machine](file:///d:/FixFlow/docs/WORK_ORDER_STATE_MACHINE.md)
- [Security Architecture](file:///d:/FixFlow/docs/SECURITY.md)
- [Role-Based Access Control (RBAC)](file:///d:/FixFlow/docs/RBAC.md)
- [Frontend Engineering Rules](file:///d:/FixFlow/docs/FRONTEND_RULES.md)
- [Backend Engineering Rules](file:///d:/FixFlow/docs/BACKEND_RULES.md)
- [Testing Strategy](file:///d:/FixFlow/docs/TESTING.md)
- [Git Workflow](file:///d:/FixFlow/docs/GIT_WORKFLOW.md)
- [Deployment Guide](file:///d:/FixFlow/docs/DEPLOYMENT.md)
- [Agent & Developer Rules](file:///d:/FixFlow/AGENTS.md)
