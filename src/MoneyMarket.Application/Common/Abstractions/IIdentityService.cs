namespace MoneyMarket.Application.Common.Abstractions
{
    public interface IIdentityService
    {
        /// <summary>Registers a user and assigns the given role.</summary>
        Task<(bool Succeeded, string? UserId, string[] Errors)> RegisterAsync(string email, string password, string role);

        /// <summary>Validates credentials and returns a JWT.</summary>
        Task<(bool Succeeded, string Token, string[] Errors)> LoginAsync(string email, string password);

        /// <summary>Adds a user to a role (id is the ASP.NET Identity user Id).</summary>
        Task AddUserToRoleAsync(Guid userId, string role, CancellationToken ct);

        /// <summary>Resolves the canonical ASP.NET Identity user Id (Guid) for an email.</summary>
        Task<Guid> GetUserIdGuidByEmailAsync(string email, CancellationToken ct);

        /// <summary>Ensures a user row exists in dbo.ApplicationUser for the given Guid Id; throws if not found.</summary>
        Task<string> EnsureUserExistsAsync(Guid userId, CancellationToken ct);
    }
}
