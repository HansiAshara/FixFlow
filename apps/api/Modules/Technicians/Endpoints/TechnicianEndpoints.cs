using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;
using FixFlow.Api.Modules.Technicians.Domain;

namespace FixFlow.Api.Modules.Technicians.Endpoints;

public record RegisterTechnicianRequest(string Bio, int YearsOfExperience, List<string> Skills);

public static class TechnicianEndpoints
{
    public static void MapTechnicianEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/technicians").WithTags("Technicians");

        group.MapGet("/", () =>
        {
            // Placeholder endpoint for public/directory view
            return Results.Ok(Array.Empty<object>());
        });

        group.MapPost("/onboard", (RegisterTechnicianRequest request, ICurrentUserProvider currentUser) =>
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId == null)
            {
                throw new ForbiddenException("Authentication required to onboard as a technician.");
            }

            var profile = new TechnicianProfile(
                Guid.NewGuid(),
                currentUser.UserId.Value,
                request.Bio,
                request.YearsOfExperience,
                request.Skills
            );

            return Results.Created($"/api/technicians/{profile.Id}", new
            {
                profile.Id,
                profile.UserId,
                Status = profile.Status.ToString(),
                profile.Skills
            });
        }).RequireAuthorization();
    }
}
