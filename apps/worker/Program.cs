using FixFlow.Worker.Abstractions;
using FixFlow.Worker.Consumers;
using FixFlow.Worker.Services;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);

// Register HTTP Client for Ollama local AI provider
builder.Services.AddHttpClient<IAiIssueAnalyzer, OllamaAiIssueAnalyzer>(client =>
{
    var baseUrl = builder.Configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
    var timeout = int.TryParse(builder.Configuration["Ollama:TimeoutSeconds"], out var s) ? s : 60;
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(timeout);
});

// Configure MassTransit RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // Register Consumers
    x.AddConsumer<IssueAnalysisRequestedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMq:Host"] ?? "localhost";
        var port = ushort.TryParse(builder.Configuration["RabbitMq:Port"], out var p) ? p : (ushort)5672;
        var user = builder.Configuration["RabbitMq:Username"] ?? "fixflow_mq";
        var pass = builder.Configuration["RabbitMq:Password"] ?? "fixflow_mq_local_password";

        cfg.Host(host, port, "/", h =>
        {
            h.Username(user);
            h.Password(pass);
        });

        // Configure standard retry policy: 3 retries with exponential interval
        cfg.UseMessageRetry(r => r.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(2)));

        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();
