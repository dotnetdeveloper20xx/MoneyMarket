using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _config;
    public JwtTokenService(IConfiguration config) => _config = config;

    public string CreateToken(string userId, string email, IEnumerable<string> roles, IDictionary<string, string>? customClaims = null)
    {
        var issuer = _config["Jwt:Issuer"] ?? "MoneyMarket";
        var audience = _config["Jwt:Audience"] ?? issuer;
        var key = _config["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key missing");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var signingCreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            // .NET-friendly claims
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, email),
            new(ClaimTypes.Email, email),

            // Standard JWT names (good for interop / FE libs)
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        foreach (var r in roles)
            claims.Add(new Claim(ClaimTypes.Role, r));

        if (customClaims is not null)
            foreach (var kv in customClaims)
                claims.Add(new Claim(kv.Key, kv.Value));

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now,                      // nbf
            expires: now.AddHours(1),            // exp
            signingCredentials: signingCreds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
