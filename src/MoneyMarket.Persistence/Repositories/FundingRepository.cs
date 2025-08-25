using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Fundings.DTOs;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories;

public sealed class FundingRepository : IFundingRepository
{
    private readonly AppDbContext _db;
    public FundingRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Funding funding, CancellationToken ct)
        => await _db.Fundings.AddAsync(funding, ct);

    public async Task<decimal> GetTotalFundedForLoanAsync(Guid loanId, CancellationToken ct)
        => await _db.Fundings.Where(f => f.LoanId == loanId).Select(f => f.Amount).SumAsync(ct);

    public async Task<IReadOnlyList<FundingSummaryDto>> GetSummariesByLenderAsync(Guid lenderId, CancellationToken ct)
        => await _db.Fundings
            .AsNoTracking()
            .Where(f => f.LenderId == lenderId)
            .OrderByDescending(f => f.CreatedAtUtc)
            .Select(f => new FundingSummaryDto(f.Id, f.LoanId, f.Amount, f.CreatedAtUtc))
            .ToListAsync(ct);

    public async Task<bool> ExistsByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct)
    {
        // Only works if your Funding entity has this property + index
        // return await _db.Fundings.AnyAsync(f => f.IdempotencyKey == idempotencyKey, ct);
        return await Task.FromResult(false); // no-op if property doesn't exist
    }
}
