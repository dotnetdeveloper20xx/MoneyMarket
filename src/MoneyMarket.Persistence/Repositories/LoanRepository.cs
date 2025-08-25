using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Loans.Dtos;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Enums;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories;

public sealed class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _db;
    public LoanRepository(AppDbContext db) => _db = db;

    public Task<Loan?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Loans.FirstOrDefaultAsync(l => l.LoanId == id, ct);

    public Task<IReadOnlyList<LoanSummaryDto>> GetOpenSummariesAsync(int page, int size, CancellationToken ct)
    {
        var skip = (page - 1) * size;

        // If you have IsVisibleToLenders, include it here:
        return _db.Loans
            .AsNoTracking()
            .Where(l => l.Status == LoanStatus.PendingFunding /* && l.IsVisibleToLenders */)
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
            .ToListAsync(ct)
            .ContinueWith<IReadOnlyList<LoanSummaryDto>>(t => t.Result, ct);
    }
}
