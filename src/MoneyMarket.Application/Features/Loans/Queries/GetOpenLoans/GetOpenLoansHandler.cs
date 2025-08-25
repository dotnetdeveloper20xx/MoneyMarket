using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Loans.Dtos;
using MoneyMarket.Application.Features.Loans.Queries.GetOpenLoans;
using MoneyMarket.Domain.Enums;

public sealed class GetOpenLoansHandler : IRequestHandler<GetOpenLoansQuery, IReadOnlyList<LoanSummaryDto>>
{
    private readonly IAppDbContext _db;
    public GetOpenLoansHandler(IAppDbContext db) => _db = db;

    public async Task<IReadOnlyList<LoanSummaryDto>> Handle(GetOpenLoansQuery request, CancellationToken ct)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var size = request.Size <= 0 ? 20 : request.Size;
        var skip = (page - 1) * size;

        return await _db.Loans
            .AsNoTracking()
            .Where(l => l.Status == LoanStatus.PendingFunding) 
            .OrderByDescending(l => l.ApplicationDateUtc)
            .Skip(skip).Take(size)
            .Select(l => new LoanSummaryDto(
                l.LoanId,
                l.Purpose,     
                (l.ApprovedAmount > 0 ? l.ApprovedAmount : l.RequestedAmount),
                l.Fundings.Sum(f => f.Amount),
                l.InterestRate,
                l.Status.ToString()
            ))
            .ToListAsync(ct);
    }
}
