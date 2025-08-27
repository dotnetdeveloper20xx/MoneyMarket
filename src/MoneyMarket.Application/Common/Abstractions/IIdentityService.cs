namespace MoneyMarket.Application.Common.Abstractions
{
    public interface IIdentityService
    {
        Task<(bool Succeeded, string? UserId, string[] Errors)> RegisterAsync(string email, string password, string role);
        Task<(bool Succeeded, string Token, string[] Errors)> LoginAsync(string email, string password);
        Task AddUserToRoleAsync(Guid userId, string role, CancellationToken ct);
    }
}
