using MoneyMarket.Api.Common.Extensions;
using MoneyMarket.Application.Common.Abstractions;
using System.Security.Claims;

namespace MoneyMarket.Api.Common.Services
{
    /// <summary>
    /// Web-layer implementation backed by IHttpContextAccessor.
    /// Keeps controllers/handlers clean and testable.
    /// </summary>
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _accessor;
        private ClaimsPrincipal? Principal => _accessor.HttpContext?.User;

        public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

        public bool IsAuthenticated => Principal?.Identity?.IsAuthenticated == true;
        public string? UserId => Principal?.UserId();
        public string? Email => Principal?.Email();

        public IReadOnlyList<string> Roles =>
            (IReadOnlyList<string>)(Principal is null ? Array.Empty<string>() : Principal.Roles());

        public bool IsInRole(string role) =>
            Roles.Contains(role, StringComparer.OrdinalIgnoreCase);

        public string GetRequiredUserId()
        {
            var id = UserId;
            if (string.IsNullOrWhiteSpace(id))
                throw new UnauthorizedAccessException("No authenticated user.");
            return id;
        }
    }
}
