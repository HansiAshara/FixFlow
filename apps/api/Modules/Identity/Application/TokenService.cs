using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FixFlow.Api.Modules.Identity.Domain;
using Microsoft.IdentityModel.Tokens;

namespace FixFlow.Api.Modules.Identity.Application;

public record AuthResponse(
    Guid Id,
    string Email,
    string FullName,
    IReadOnlyList<string> Roles,
    string AccessToken,
    DateTime ExpiresAtUtc
);

public record RegisterRequest(string Email, string Password, string FullName);
public record LoginRequest(string Email, string Password);

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles);
}

public class JwtTokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles)
    {
        var secretKey = configuration["Jwt:SecretKey"] ?? "FixFlow_Super_Secure_Local_Development_Key_At_Least_32_Bytes_Long_2026!";
        var issuer = configuration["Jwt:Issuer"] ?? "FixFlow.Api";
        var audience = configuration["Jwt:Audience"] ?? "FixFlow.Web";
        var expiryMinutes = int.TryParse(configuration["Jwt:ExpiryMinutes"], out var exp) ? exp : 60;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new("fullName", user.FullName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
