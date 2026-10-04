using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using FixFlow.Worker.Abstractions;

namespace FixFlow.Worker.Services;

public class OllamaAiIssueAnalyzer(
    HttpClient httpClient,
    IConfiguration configuration,
    ILogger<OllamaAiIssueAnalyzer> logger)
    : IAiIssueAnalyzer
{
    private static readonly HashSet<string> ValidCategories = new(StringComparer.OrdinalIgnoreCase)
    {
        "Plumbing", "Electrical", "HVAC", "Carpentry", "Appliances", "Roofing", "Masonry", "GeneralMaintenance"
    };

    private static readonly HashSet<string> ValidSeverities = new(StringComparer.OrdinalIgnoreCase)
    {
        "LOW", "MEDIUM", "HIGH", "CRITICAL"
    };

    private static readonly HashSet<string> ValidRiskLevels = new(StringComparer.OrdinalIgnoreCase)
    {
        "LOW", "MEDIUM", "HIGH", "HAZARDOUS"
    };

    public async Task<AiIssueAnalysisResult> AnalyzeIssueAsync(
        string rawDescription,
        string location,
        CancellationToken cancellationToken = default)
    {
        var model = configuration["Ollama:Model"] ?? "llama3.2:3b";
        logger.LogInformation("Submitting issue analysis request to Ollama using model {Model}", model);

        var prompt = $$"""
        You are FixFlow's maintenance issue classifier. Analyze the following maintenance issue report and return ONLY valid JSON matching this schema:
        {
          "category": "Plumbing|Electrical|HVAC|Carpentry|Appliances|Roofing|Masonry|GeneralMaintenance",
          "requiredSkills": ["skill1", "skill2"],
          "severity": "LOW|MEDIUM|HIGH|CRITICAL",
          "riskLevel": "LOW|MEDIUM|HIGH|HAZARDOUS",
          "tags": ["tag1", "tag2"],
          "confidence": 0.95
        }

        Issue location: {{location}}
        Issue description: {{rawDescription}}

        JSON Response:
        """;

        try
        {
            var payload = new
            {
                model,
                prompt,
                stream = false,
                format = "json"
            };

            var response = await httpClient.PostAsJsonAsync("/api/generate", payload, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<OllamaGenerateResponse>(cancellationToken: cancellationToken);
                if (!string.IsNullOrWhiteSpace(content?.Response))
                {
                    var parsed = JsonSerializer.Deserialize<RawAiResponse>(content.Response, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (parsed != null)
                    {
                        return SanitizeAndValidate(parsed);
                    }
                }
            }

            logger.LogWarning("Ollama returned non-success or empty response. Falling back to deterministic rule extraction.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to communicate with local Ollama service. Falling back to rule extraction.");
        }

        return FallbackExtraction(rawDescription);
    }

    private static AiIssueAnalysisResult SanitizeAndValidate(RawAiResponse raw)
    {
        // Treat AI output as UNTRUSTED input: sanitize, validate categories, clamp confidence
        var category = ValidCategories.Contains(raw.Category ?? "") ? raw.Category! : "GeneralMaintenance";
        var severity = ValidSeverities.Contains(raw.Severity ?? "") ? raw.Severity!.ToUpperInvariant() : "MEDIUM";
        var risk = ValidRiskLevels.Contains(raw.RiskLevel ?? "") ? raw.RiskLevel!.ToUpperInvariant() : "LOW";
        var skills = (raw.RequiredSkills ?? []).Where(s => !string.IsNullOrWhiteSpace(s)).Take(5).ToList();
        if (skills.Count == 0)
        {
            skills.Add("General Inspection");
        }

        var tags = (raw.Tags ?? []).Where(t => !string.IsNullOrWhiteSpace(t)).Take(10).ToList();
        var confidence = Math.Clamp(raw.Confidence ?? 0.8, 0.0, 1.0);

        return new AiIssueAnalysisResult(category, skills, severity, risk, tags, confidence);
    }

    private static AiIssueAnalysisResult FallbackExtraction(string text)
    {
        var lower = text.ToLowerInvariant();
        if (lower.Contains("leak") || lower.Contains("pipe") || lower.Contains("water") || lower.Contains("drain"))
        {
            return new AiIssueAnalysisResult("Plumbing", ["Pipe Repair", "Leak Detection"], "HIGH", "MEDIUM", ["plumbing", "water"], 0.75);
        }

        if (lower.Contains("spark") || lower.Contains("wire") || lower.Contains("breaker") || lower.Contains("electric"))
        {
            return new AiIssueAnalysisResult("Electrical", ["Circuit Diagnosis", "Wiring"], "CRITICAL", "HAZARDOUS", ["electrical", "safety-risk"], 0.85);
        }

        return new AiIssueAnalysisResult("GeneralMaintenance", ["General Inspection"], "MEDIUM", "LOW", ["maintenance"], 0.50);
    }

    private sealed record OllamaGenerateResponse(string Response);

    private sealed record RawAiResponse(
        string? Category,
        List<string>? RequiredSkills,
        string? Severity,
        string? RiskLevel,
        List<string>? Tags,
        double? Confidence
    );
}
