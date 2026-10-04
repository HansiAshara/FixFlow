# FixFlow - Database Design & Schema Ownership

## 1. Single PostgreSQL Instance Policy
FixFlow V1 uses a single PostgreSQL database (`fixflow_db`) managed via EF Core Migrations.

## 2. Table Ownership by Module

| Module | Primary Tables | Responsibility |
| :--- | :--- | :--- |
| **Identity** | `AspNetUsers`, `AspNetRoles`, `AspNetUserRoles` | User credentials, security stamps, logins |
| **Issues** | `issues` | Problem statements, AI category/tags, priority |
| **Technicians** | `technician_profiles`, `technician_evidence` | Professional bios, skills, license evidence |
| **WorkOrders** | `work_orders` | Work order status, timestamps, notes |
| **Resolutions** | `resolutions` | Verification ratings, feedback, reopen audits |
| **Performance** | `technician_performance` | Aggregated scores, completion/response rates |
| **Notifications** | `notifications` | In-app alerts, read status |
| **Administration**| `admin_audit_logs` | Human verification audit trails |

## 3. Storage Rules
- **UTC Timestamps**: All timestamps stored in UTC (`timestamp with time zone`).
- **PostgreSQL Arrays**: Native text arrays (`text[]`) used for skill tags and issue categories.
- **Foreign Key Integrity**: Cross-module references use GUID keys. Direct joins across non-owned modules in application logic are forbidden to preserve isolation.
