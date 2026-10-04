# FixFlow - Role-Based & Resource-Based Authorization (RBAC)

## 1. System Roles
1. `USER`: Standard authenticated property owner or tenant reporting maintenance issues.
2. `TECHNICIAN`: Verified or probationary service provider performing work orders.
3. `ADMIN`: System administrator handling human-in-the-loop verification and platform compliance.

A `USER` can apply to become a `TECHNICIAN` without needing a separate user account.

---

## 2. Authorization Rules: RBAC + Resource Ownership + Business State

Authorization decisions must never rely on role checks alone. The backend authoritatively verifies:
$$\text{Authorized} = \text{Role Check} \land \text{Resource Ownership} \land \text{Valid Domain State}$$

### Concrete Permission Rules:
- **Report Issue**: Any authenticated user (`USER`, `TECHNICIAN`, `ADMIN`).
- **Modify/Cancel Issue**: Issue Reporter + Issue Status is `Reported` or `Draft`.
- **Review Qualification Evidence**: `ADMIN` only.
- **Accept/Reject Work Order**: Assigned Technician + Work Order is `AWAITING_RESPONSE`.
- **Start Work**: Assigned Technician + Work Order is `ACCEPTED` or `READY_TO_START`.
- **Complete Work**: Assigned Technician + Work Order is `IN_PROGRESS`.
- **Verify or Reopen Resolution**: Issue Reporter + Work Order is `AWAITING_REPORTER`.

---

## 3. Frontend Non-Authority
Frontend `AuthGuard` or `RoleGuard` components are purely cosmetic/UX mechanisms to provide clean client navigation. The ASP.NET Core API strictly validates claims on every HTTP request and never trusts user IDs or role claims passed in request payloads.
