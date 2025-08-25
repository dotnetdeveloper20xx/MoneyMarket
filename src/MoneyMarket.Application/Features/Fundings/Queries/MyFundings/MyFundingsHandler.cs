using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Fundings.DTOs;

namespace MoneyMarket.Application.Features.Fundings.Queries.MyFundings
{
    public sealed class MyFundingsHandler : IRequestHandler<MyFundingsQuery, IReadOnlyList<FundingSummaryDto>>
    {
        private readonly IAppDbContext _db;
        public MyFundingsHandler(IAppDbContext db) => _db = db;

        public async Task<IReadOnlyList<FundingSummaryDto>> Handle(MyFundingsQuery req, CancellationToken ct)
        {
            return await _db.Fundings
                .Where(f => f.LenderId == req.LenderId)
                .OrderByDescending(f => f.CreatedAtUtc)
                .Select(f => new FundingSummaryDto(f.Id, f.LoanId, f.Amount, f.CreatedAtUtc))
                .ToListAsync(ct);
        }
    }
}
