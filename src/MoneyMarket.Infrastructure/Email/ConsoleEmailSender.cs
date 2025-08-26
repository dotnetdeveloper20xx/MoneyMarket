using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Infrastructure.Email
{
    public sealed class ConsoleEmailSender : IEmailSender
    {
        public Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
        {
            Console.WriteLine($"[EMAIL] To: {toEmail}, Subject: {subject}\n{htmlBody}");
            return Task.CompletedTask;
        }
    }
}
