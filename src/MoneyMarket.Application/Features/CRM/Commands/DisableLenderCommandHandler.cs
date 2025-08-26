using MediatR;
using MoneyMarket.Application.Common.Abstractions;

namespace MoneyMarket.Application.Features.CRM.Commands
{
    public sealed class DisableLenderCommandHandler : IRequestHandler<DisableLenderCommand, bool>
    {
        private readonly ILenderProfileRepository _repo;
        private readonly INotificationService _notify;


        public DisableLenderCommandHandler(ILenderProfileRepository repo, INotificationService notify)
        => (_repo, _notify) = (repo, notify);


        public async Task<bool> Handle(DisableLenderCommand request, CancellationToken ct)
        {
            var l = await _repo.GetByIdAsync(request.LenderId, ct);
            if (l is null) return false;


            l.Disable(request.Reason);
            await _repo.SaveChangesAsync(ct);


            // Notifications
            await _notify.NotifyRoleAsync(Domain.Common.Roles.Admin, $"Lender {l.Email} has been disabled by CRM.", ct);
            await _notify.NotifyUserAsync(l.UserId, "Your account for further funding has been put on hold.", ct);


            return true;
        }
    }
}
