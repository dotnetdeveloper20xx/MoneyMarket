using MediatR;
using MoneyMarket.Application.Common.Messaging;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Fundings.Commands.FundLoan
{
    public sealed record FundLoanCommand(
         Guid LoanId,
         decimal Amount,
         string? IdempotencyKey
     ) : IRequest<ApiResponse<bool>>, ITransactionalRequest; // <- UnitOfWorkBehavior commits
}
