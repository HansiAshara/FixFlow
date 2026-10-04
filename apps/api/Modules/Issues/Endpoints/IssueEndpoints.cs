using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Modules.Issues.Application.CreateIssue;
using FluentValidation;

namespace FixFlow.Api.Modules.Issues.Endpoints;

public static class IssueEndpoints
{
    public static void MapIssueEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/issues").WithTags("Issues").RequireAuthorization();

        group.MapPost("/", async (
            CreateIssueCommand command,
            IValidator<CreateIssueCommand> validator,
            CreateIssueHandler handler,
            CancellationToken ct) =>
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.ToDictionary();
                throw new FixFlow.Api.Common.Exceptions.ValidationException(errors);
            }

            var result = await handler.HandleAsync(command, ct);
            return Results.Created($"/api/issues/{result.Value.IssueId}", result.Value);
        });

        group.MapGet("/", () =>
        {
            // Placeholder endpoint for listing user issues
            return Results.Ok(Array.Empty<object>());
        });
    }
}
