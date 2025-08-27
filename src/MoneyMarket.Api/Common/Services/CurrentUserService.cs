using MoneyMarket.Application.Common.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MoneyMarket.Api.Common.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? UserId =>
      _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
      ?? _accessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
    public string? Email =>
        _accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email)
        ?? _accessor.HttpContext?.User.FindFirstValue(JwtRegisteredClaimNames.Email);
    public IReadOnlyList<string> Roles => User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;

    public string GetRequiredUserId()
        => UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");
}
