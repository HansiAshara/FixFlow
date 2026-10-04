using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;

namespace FixFlow.Api.Modules.Notifications.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/notifications").WithTags("Notifications").RequireAuthorization();

        group.MapGet("/", (ICurrentUserProvider currentUser) =>
        {
            if (!currentUser.IsAuthenticated)
            {
                throw new ForbiddenException();
            }

            return Results.Ok(Array.Empty<object>());
        });

        group.MapPost("/{id:guid}/read", (Guid id, ICurrentUserProvider currentUser) =>
        {
            return Results.Ok(new { Id = id, IsRead = true });
        });
    }
}
