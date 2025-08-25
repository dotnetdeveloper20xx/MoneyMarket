using MoneyMarket.Application.Features.Loans.Dtos;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Application.Common.Abstractions;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IReadOnlyList<LoanSummaryDto>> GetOpenSummariesAsync(int page, int size, CancellationToken ct);
}
