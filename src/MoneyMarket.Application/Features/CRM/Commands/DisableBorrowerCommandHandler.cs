using MediatR;
using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Application.Features.CRM.Commands
{
    public sealed class DisableBorrowerCommandHandler : IRequestHandler<DisableBorrowerCommand, bool>
    {
        private readonly IBorrowerRepository _repo;
        private readonly INotificationService _notify;
        public DisableBorrowerCommandHandler(IBorrowerRepository repo, INotificationService notify)
            => (_repo, _notify) = (repo, notify);

        public async Task<bool> Handle(DisableBorrowerCommand request, CancellationToken ct)
        {
            var b = await _repo.GetByIdAsync(request.BorrowerId, ct);
            if (b is null) return false;

            b.Disable(request.Reason);
            await _repo.SaveChangesAsync(ct);

            await _notify.NotifyRoleAsync("Admin", $"Borrower {b.Email} has been disabled by CRM.", ct);
            await _notify.NotifyUserAsync(b.FirstName, "Your account has been put on hold. You will not be able to apply for loans.", ct);
            return true;
        }
    }
}
