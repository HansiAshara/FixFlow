# FixFlow - Security Architecture & File Upload Guidelines

## 1. Authentication Security
- **ASP.NET Core Identity & JWT**: Secure cryptographic JWT bearer tokens signed with HMAC-SHA256 (32+ byte keys).
- **Password Policies**: Mandatory 8+ character complexity with hashed storage using PBKDF2 with SHA-512 salt.
- **Rate Limiting**: Rate limiters applied to `/api/auth/login` and `/api/auth/register`.

---

## 2. Server-Side File Upload Guidelines
When file attachments (evidence, images, certificates) are processed in future feature milestones:
1. **MIME Allowlist**: Strictly enforce allowed image/document MIME types (`image/jpeg`, `image/png`, `image/webp`, `application/pdf`).
2. **Rejection of Executables**: Reject `.exe`, `.dll`, `.sh`, `.bat`, `.cmd`, `.js`, `.vbs`, etc.
3. **Server-Generated Filenames**: Never store files using client-supplied file names. Generate unique GUID filenames (e.g. `evidence_{guid}.pdf`).
4. **Size Caps**: Cap uploads at 10MB per file.
5. **Authorization Checks**: Access to uploaded documents must verify caller ownership or admin role.
