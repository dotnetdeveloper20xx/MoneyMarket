using Microsoft.Extensions.Logging;
using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Infrastructure.Notifications
{
    public sealed class NoOpNotificationService : INotificationService
    {
        private readonly ILogger<NoOpNotificationService> _logger;
        public NoOpNotificationService(ILogger<NoOpNotificationService> logger) => _logger = logger;


        public Task NotifyRoleAsync(string roleName, string message, CancellationToken ct = default)
        {
            _logger.LogInformation("[NotifyRole] Role={Role} Message={Message}", roleName, message);
            return Task.CompletedTask;
        }


        public Task NotifyUserAsync(string userId, string message, CancellationToken ct = default)
        {
            _logger.LogInformation("[NotifyUser] UserId={UserId} Message={Message}", userId, message);
            return Task.CompletedTask;
        }
    }
}
