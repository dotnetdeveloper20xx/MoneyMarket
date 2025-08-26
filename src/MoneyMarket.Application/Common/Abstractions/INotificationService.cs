namespace MoneyMarket.Application.Common.Abstractions
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string message, CancellationToken ct = default);
        Task NotifyRoleAsync(string roleName, string message, CancellationToken ct = default);
    }
}
