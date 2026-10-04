using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;
using FixFlow.Api.Modules.WorkOrders.Domain;

namespace FixFlow.Api.Modules.WorkOrders.Endpoints;

public record CreateWorkOrderRequest(Guid IssueId, Guid TechnicianId);
public record RejectWorkOrderRequest(string Reason);
public record CompleteWorkOrderRequest(string CompletionNotes);

public static class WorkOrderEndpoints
{
    public static void MapWorkOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/work-orders").WithTags("WorkOrders").RequireAuthorization();

        group.MapPost("/", (CreateWorkOrderRequest request, ICurrentUserProvider currentUser) =>
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId == null)
            {
                throw new ForbiddenException();
            }

            var workOrder = new WorkOrder(
                Guid.NewGuid(),
                request.IssueId,
                currentUser.UserId.Value,
                request.TechnicianId,
                TimeSpan.FromHours(24)
            );

            return Results.Created($"/api/work-orders/{workOrder.Id}", new
            {
                workOrder.Id,
                workOrder.IssueId,
                workOrder.AssignedTechnicianId,
                Status = workOrder.Status.ToString(),
                workOrder.AwaitingResponseUntilUtc
            });
        });

        group.MapPost("/{id:guid}/accept", (Guid id, ICurrentUserProvider currentUser) =>
        {
            return Results.Ok(new { Id = id, Status = "Accepted" });
        });

        group.MapPost("/{id:guid}/reject", (Guid id, RejectWorkOrderRequest request, ICurrentUserProvider currentUser) =>
        {
            return Results.Ok(new { Id = id, Status = "Rejected", request.Reason });
        });

        group.MapPost("/{id:guid}/start", (Guid id, ICurrentUserProvider currentUser) =>
        {
            return Results.Ok(new { Id = id, Status = "InProgress" });
        });

        group.MapPost("/{id:guid}/complete", (Guid id, CompleteWorkOrderRequest request, ICurrentUserProvider currentUser) =>
        {
            return Results.Ok(new { Id = id, Status = "AwaitingReporter" });
        });

        group.MapGet("/", () =>
        {
            return Results.Ok(Array.Empty<object>());
        });
    }
}
