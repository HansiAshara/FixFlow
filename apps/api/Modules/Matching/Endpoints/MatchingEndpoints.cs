using FixFlow.Api.Modules.Matching.Domain;

namespace FixFlow.Api.Modules.Matching.Endpoints;

public static class MatchingEndpoints
{
    public static void MapMatchingEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/matching").WithTags("Matching").RequireAuthorization();

        group.MapGet("/recommendations/{issueId:guid}", (Guid issueId) =>
        {
            // Placeholder endpoint returning ranking structure skeleton
            var sample = new List<RankedTechnician>
            {
                new(
                    Guid.NewGuid(),
                    "Alex Smith (Master Electrician)",
                    0.94,
                    1.0,
                    0.9,
                    0.95,
                    0.92,
                    0.93,
                    "VERIFIED"
                ),
                new(
                    Guid.NewGuid(),
                    "Jordan Lee (HVAC & Electrical)",
                    0.88,
                    0.85,
                    0.9,
                    0.9,
                    0.89,
                    0.86,
                    "PROBATIONARY"
                )
            };

            return Results.Ok(sample);
        });
    }
}
