using MoneyMarket.Application.Features.Fundings.DTOs;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface IFundingRepository   
    {
        Task AddAsync(Funding funding, CancellationToken ct);
        Task<decimal> GetTotalFundedForLoanAsync(Guid loanId, CancellationToken ct);
        Task<IReadOnlyList<FundingSummaryDto>> GetSummariesByLenderAsync(Guid lenderId, CancellationToken ct);
        Task<bool> ExistsByIdempotencyKeyAsync(string idempotencyKey, CancellationToken ct);
    }
}
