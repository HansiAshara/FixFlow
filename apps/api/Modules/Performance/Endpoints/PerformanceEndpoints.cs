namespace FixFlow.Api.Modules.Performance.Endpoints;

public static class PerformanceEndpoints
{
    public static void MapPerformanceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/performance").WithTags("Performance").RequireAuthorization();

        group.MapGet("/technician/{technicianId:guid}", (Guid technicianId) =>
        {
            return Results.Ok(new
            {
                TechnicianId = technicianId,
                HistoricalScore = 0.92,
                ResponseRate = 0.98,
                CompletionRate = 0.95,
                TotalCompletedJobs = 42,
                AverageRating = 4.8
            });
        });
    }
}
