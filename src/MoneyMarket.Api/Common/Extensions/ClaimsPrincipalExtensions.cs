using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace MoneyMarket.Api.Common.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? UserId(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            user.FindFirstValue(JwtRegisteredClaimNames.Sub);

        public static string? Email(this ClaimsPrincipal user) =>
            user.FindFirstValue(ClaimTypes.Email) ??
            user.FindFirstValue(JwtRegisteredClaimNames.Email);

        public static string[] Roles(this ClaimsPrincipal user) =>
            user.FindAll(ClaimTypes.Role).Select(c => c.Value).Distinct().ToArray();
    }
}
