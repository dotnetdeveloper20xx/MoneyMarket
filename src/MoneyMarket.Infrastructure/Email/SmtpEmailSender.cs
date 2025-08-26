using Microsoft.Extensions.Configuration;
using MoneyMarket.Application.Common.Abstractions;
using System.Net;
using System.Net.Mail;

namespace MoneyMarket.Infrastructure.Email
{
    public sealed class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _client;
        private readonly string _from;

        public SmtpEmailSender(IConfiguration config)
        {
            var host = config["Smtp:Host"] ?? throw new InvalidOperationException("SMTP host missing");
            var port = int.Parse(config["Smtp:Port"] ?? "587");
            var user = config["Smtp:User"] ?? throw new InvalidOperationException("SMTP user missing");
            var pass = config["Smtp:Pass"] ?? throw new InvalidOperationException("SMTP password missing");
            _from = config["Smtp:From"] ?? user;

            _client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(user, pass),
                EnableSsl = true
            };
        }

        public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken ct)
        {
            using var msg = new MailMessage(_from, toEmail, subject, htmlBody) { IsBodyHtml = true };
            await _client.SendMailAsync(msg, ct);
        }
    }
}
