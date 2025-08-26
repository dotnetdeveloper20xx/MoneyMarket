using MediatR;
using Microsoft.Extensions.Configuration;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Borrowers.Events;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class ProfileSubmittedHandler : INotificationHandler<ProfileSubmittedEvent>
    {
        private readonly IEmailSender _email;
        private readonly IConfiguration _cfg;

        public ProfileSubmittedHandler(IEmailSender email, IConfiguration cfg)
        { _email = email; _cfg = cfg; }

        public async Task Handle(ProfileSubmittedEvent notification, CancellationToken ct)
        {
            var riskEmail = _cfg["CreditRisk:Email"] ?? "creditrisk@example.com";
            var subject = $"Borrower profile submitted: {notification.UserId}";
            var body = $@"<p>Borrower profile <strong>{notification.BorrowerProfileId}</strong> has been submitted.</p>
                     <p>User: {notification.UserId} ({notification.Email})</p>";
            await _email.SendAsync(riskEmail, subject, body, ct);
        }
    }
}
