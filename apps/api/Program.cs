using System.Text;
using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Middleware;
using FixFlow.Api.Common.Security;
using FixFlow.Api.Infrastructure.Persistence;
using FixFlow.Api.Modules.Administration.Endpoints;
using FixFlow.Api.Modules.Identity.Application;
using FixFlow.Api.Modules.Identity.Domain;
using FixFlow.Api.Modules.Identity.Endpoints;
using FixFlow.Api.Modules.Issues.Application.CreateIssue;
using FixFlow.Api.Modules.Issues.Endpoints;
using FixFlow.Api.Modules.Matching.Endpoints;
using FixFlow.Api.Modules.Notifications.Endpoints;
using FixFlow.Api.Modules.Performance.Endpoints;
using FixFlow.Api.Modules.Resolutions.Endpoints;
using FixFlow.Api.Modules.Technicians.Endpoints;
using FixFlow.Api.Modules.WorkOrders.Endpoints;
using FluentValidation;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Global Exception Handler & ProblemDetails
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Database
builder.Services.AddDbContext<FixFlowDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly(typeof(FixFlowDbContext).Assembly.GetName().Name);
        npgsqlOptions.EnableRetryOnFailure(3);
    });
});

// Identity
builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;
    options.User.RequireUniqueEmail = true;
})
.AddRoles<ApplicationRole>()
.AddEntityFrameworkStores<FixFlowDbContext>()
.AddDefaultTokenProviders();

// Security / JWT
var jwtSecret = builder.Configuration["Jwt:SecretKey"] ?? "FixFlow_Super_Secure_Local_Development_Key_At_Least_32_Bytes_Long_2026!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "FixFlow.Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "FixFlow.Web";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.FromMinutes(1)
    };
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AppPolicies.RequireUser, policy => policy.RequireRole(AppRoles.User, AppRoles.Technician, AppRoles.Admin))
    .AddPolicy(AppPolicies.RequireTechnician, policy => policy.RequireRole(AppRoles.Technician, AppRoles.Admin))
    .AddPolicy(AppPolicies.RequireAdmin, policy => policy.RequireRole(AppRoles.Admin));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// MassTransit with RabbitMQ
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

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

        cfg.ConfigureEndpoints(context);
    });
});

// FluentValidation & CQRS Handlers
builder.Services.AddValidatorsFromAssemblyContaining<CreateIssueValidator>();
builder.Services.AddScoped<CreateIssueHandler>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowWebClient");

app.UseAuthentication();
app.UseAuthorization();

// Health Check endpoints
app.MapGet("/health/live", () => Results.Ok(new { status = "Healthy", timestamp = DateTime.UtcNow }));
app.MapGet("/health/ready", () => Results.Ok(new { status = "Ready", timestamp = DateTime.UtcNow }));

// Map Module Endpoints
app.MapAuthEndpoints();
app.MapIssueEndpoints();
app.MapMatchingEndpoints();
app.MapTechnicianEndpoints();
app.MapWorkOrderEndpoints();
app.MapResolutionEndpoints();
app.MapPerformanceEndpoints();
app.MapNotificationEndpoints();
app.MapAdminEndpoints();

app.Run();

// Required for WebApplicationFactory in Integration Tests
public partial class Program { }
