using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class UpsertLenderFinancialHandler
         : IRequestHandler<UpsertLenderFinancialCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _repo;
        private readonly ICurrentUserService _current;

        public UpsertLenderFinancialHandler(ILenderApplicationRepository repo, ICurrentUserService current)
            => (_repo, _current) = (repo, current);

        public async Task<LenderApplicationSummaryDto> Handle(UpsertLenderFinancialCommand request, CancellationToken ct)
        {           

            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");
            var actor = _current.Email ?? "system";

            var app = await _repo.GetMineAsync(userId, asNoTracking: false, ct)
                      ?? LenderApplication.Start(userId, _current.Email ?? string.Empty);

            var d = request.Dto;
            var info = new FinancialCapacityInfo(
                d.FundingSourceType,
                d.FundingSourceDescription,
                d.CapitalReserveDocuments);

            app.UpsertFinancialCapacity(info, actor);

            if (app.CreatedAtUtc == default)
                await _repo.AddAsync(app, ct);
            else
                _repo.Update(app);

            await _repo.SaveChangesAsync(ct);

            return ToSummary(app);
        }

        private static LenderApplicationSummaryDto ToSummary(LenderApplication app) =>
            new(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
    }
}
