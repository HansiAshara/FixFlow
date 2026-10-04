# FixFlow - API Contracts Specification

All endpoints follow RFC 7807 `ProblemDetails` on errors. All authenticated endpoints require `Authorization: Bearer <token>`.

---

## 1. Authentication (`/api/auth`)
- `POST /api/auth/register`: `{ email, password, fullName }` -> `200 OK { id, email, fullName, roles, accessToken }`
- `POST /api/auth/login`: `{ email, password }` -> `200 OK { id, email, fullName, roles, accessToken }`
- `GET /api/auth/me`: `200 OK { userId, email, roles }`

---

## 2. Issues (`/api/issues`)
- `POST /api/issues`: `{ title, rawDescription, location }` -> `201 Created { issueId, title, status, createdAtUtc }`
- `GET /api/issues`: Returns list of reported issues for current user.

---

## 3. Matching (`/api/matching`)
- `GET /api/matching/recommendations/{issueId}`: Returns ranked list of eligible technicians with match scores.

---

## 4. Technicians (`/api/technicians`)
- `GET /api/technicians`: Public directory of verified & probationary technicians.
- `POST /api/technicians/onboard`: `{ bio, yearsOfExperience, skills }` -> `201 Created`

---

## 5. Work Orders (`/api/work-orders`)
- `POST /api/work-orders`: `{ issueId, technicianId }` -> `201 Created`
- `POST /api/work-orders/{id}/accept`: Assigned technician accepts.
- `POST /api/work-orders/{id}/reject`: `{ reason }` -> Assigned technician rejects.
- `POST /api/work-orders/{id}/start`: Assigned technician starts work.
- `POST /api/work-orders/{id}/complete`: `{ completionNotes }` -> Assigned technician marks completed.

---

## 6. Resolutions (`/api/resolutions`)
- `POST /api/resolutions/{workOrderId}/verify`: `{ rating, feedback }` -> Reporter verifies work.
- `POST /api/resolutions/{workOrderId}/reopen`: `{ reason }` -> Reporter reopens work.

---

## 7. Administration (`/api/admin`)
- `GET /api/admin/pending-technicians`: List pending evidence.
- `POST /api/admin/technicians/{techId}/evidence/{evId}/verify`: `{ approved, reviewNotes }`
