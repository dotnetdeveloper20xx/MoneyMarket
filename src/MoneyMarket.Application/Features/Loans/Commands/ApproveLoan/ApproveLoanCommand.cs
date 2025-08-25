using MediatR;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Common.Messaging;

namespace MoneyMarket.Application.Features.Loans.Commands.ApproveLoan
{
    public sealed record ApproveLoanCommand(
        Guid LoanId,
        decimal ApprovedAmount,
        decimal InterestRate,
        decimal Fees
    ) : IRequest<ApiResponse<bool>>, ITransactionalRequest; // <- commit via UnitOfWorkBehavior
}
