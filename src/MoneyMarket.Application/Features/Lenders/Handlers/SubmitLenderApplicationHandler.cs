using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class SubmitLenderApplicationHandler
   : IRequestHandler<SubmitLenderApplicationCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _repo;
        private readonly ICurrentUserService _current;

        public SubmitLenderApplicationHandler(ILenderApplicationRepository repo, ICurrentUserService current)
            => (_repo, _current) = (repo, current);

        public async Task<LenderApplicationSummaryDto> Handle(SubmitLenderApplicationCommand request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");
            var email = _current.Email!;
            var app = await _repo.GetMineAsync(userId, asNoTracking: false, ct)
                      ?? throw new InvalidOperationException("No lender application found.");

            app.Submit(email);
            _repo.Update(app);
          

            return new LenderApplicationSummaryDto(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
