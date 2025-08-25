using MediatR;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Fundings.Commands.FundLoan
{
    public sealed record FundLoanCommand(Guid LoanId, Guid LenderId, decimal Amount, string? IdempotencyKey)
     : IRequest<ApiResponse<bool>>;
}
