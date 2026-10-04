using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;

namespace FixFlow.Api.Modules.Administration.Endpoints;

public record VerifyTechnicianEvidenceRequest(bool Approved, string? ReviewNotes);

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Administration")
            .RequireAuthorization(AppPolicies.RequireAdmin);

        group.MapGet("/pending-technicians", () =>
        {
            return Results.Ok(Array.Empty<object>());
        });

        group.MapPost("/technicians/{technicianId:guid}/evidence/{evidenceId:guid}/verify", (
            Guid technicianId,
            Guid evidenceId,
            VerifyTechnicianEvidenceRequest request,
            ICurrentUserProvider currentUser) =>
        {
            // Human-in-the-loop verification by Admin
            return Results.Ok(new
            {
                TechnicianId = technicianId,
                EvidenceId = evidenceId,
                request.Approved,
                VerifiedByAdminId = currentUser.UserId,
                VerifiedAtUtc = DateTime.UtcNow
            });
        });
    }
}
