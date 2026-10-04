using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;
using FixFlow.Api.Modules.Resolutions.Domain;

namespace FixFlow.Api.Modules.Resolutions.Endpoints;

public record VerifyResolutionRequest(int Rating, string? Feedback);
public record ReopenResolutionRequest(string Reason);

public static class ResolutionEndpoints
{
    public static void MapResolutionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/resolutions").WithTags("Resolutions").RequireAuthorization();

        group.MapPost("/{workOrderId:guid}/verify", (Guid workOrderId, VerifyResolutionRequest request, ICurrentUserProvider currentUser) =>
        {
            if (!currentUser.IsAuthenticated)
            {
                throw new ForbiddenException();
            }

            return Results.Ok(new { WorkOrderId = workOrderId, Status = "Verified", request.Rating });
        });

        group.MapPost("/{workOrderId:guid}/reopen", (Guid workOrderId, ReopenResolutionRequest request, ICurrentUserProvider currentUser) =>
        {
            if (!currentUser.IsAuthenticated)
            {
                throw new ForbiddenException();
            }

            return Results.Ok(new { WorkOrderId = workOrderId, Status = "Reopened", request.Reason });
        });
    }
}
