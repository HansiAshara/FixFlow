using FixFlow.Api.Common.Domain;
using FixFlow.Api.Common.Exceptions;
using FixFlow.Api.Common.Security;
using FixFlow.Api.Modules.Identity.Application;
using FixFlow.Api.Modules.Identity.Domain;
using Microsoft.AspNetCore.Identity;

namespace FixFlow.Api.Modules.Identity.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Authentication");

        group.MapPost("/register", async (
            RegisterRequest request,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ITokenService tokenService,
            CancellationToken ct) =>
        {
            var existingUser = await userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ConflictException("User with this email already exists.");
            }

            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                CreatedAtUtc = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.GroupBy(e => e.Code).ToDictionary(g => g.Key, g => g.Select(e => e.Description).ToArray());
                throw new FixFlow.Api.Common.Exceptions.ValidationException(errors);
            }

            // Ensure USER role exists and assign
            if (!await roleManager.RoleExistsAsync(AppRoles.User))
            {
                await roleManager.CreateAsync(new ApplicationRole(AppRoles.User));
            }
            await userManager.AddToRoleAsync(user, AppRoles.User);

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenService.GenerateAccessToken(user, roles);

            return Results.Ok(new AuthResponse(user.Id, user.Email, user.FullName, roles.ToList(), token, DateTime.UtcNow.AddHours(1)));
        });

        group.MapPost("/login", async (
            LoginRequest request,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            CancellationToken ct) =>
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!user.IsActive)
            {
                throw new ForbiddenException("Account has been suspended.");
            }

            user.LastLoginAtUtc = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            var roles = await userManager.GetRolesAsync(user);
            var token = tokenService.GenerateAccessToken(user, roles);

            return Results.Ok(new AuthResponse(user.Id, user.Email ?? string.Empty, user.FullName, roles.ToList(), token, DateTime.UtcNow.AddHours(1)));
        });

        group.MapGet("/me", (ICurrentUserProvider currentUser) =>
        {
            if (!currentUser.IsAuthenticated || currentUser.UserId == null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new
            {
                currentUser.UserId,
                currentUser.Email,
                currentUser.Roles
            });
        }).RequireAuthorization();
    }
}
