using System.Security.Claims;

namespace FixFlow.Api.Common.Security;

public static class AppRoles
{
    public const string User = "USER";
    public const string Technician = "TECHNICIAN";
    public const string Admin = "ADMIN";
}

public static class AppPolicies
{
    public const string RequireUser = "RequireUser";
    public const string RequireTechnician = "RequireTechnician";
    public const string RequireAdmin = "RequireAdmin";
}

public interface ICurrentUserProvider
{
    Guid? UserId { get; }
    string? Email { get; }
    IReadOnlyList<string> Roles { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
}

public class CurrentUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    public Guid? UserId
    {
        get
        {
            var sub = User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue("sub");
            return Guid.TryParse(sub, out var id) ? id : null;
        }
    }

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public IReadOnlyList<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? [];

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}
