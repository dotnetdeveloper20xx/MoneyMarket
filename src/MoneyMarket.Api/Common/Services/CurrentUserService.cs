using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Api.Common.Services;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? User?.FindFirstValue("sub");
    public string? Email => User?.FindFirstValue(ClaimTypes.Email);
    public IReadOnlyList<string> Roles => User?.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList() ?? new List<string>();

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;

    public string GetRequiredUserId()
        => UserId ?? throw new UnauthorizedAccessException("User is not authenticated.");
}
