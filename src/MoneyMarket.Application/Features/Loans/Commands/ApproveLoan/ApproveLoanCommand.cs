using MediatR;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Loans.Commands.ApproveLoan
{
    public sealed record ApproveLoanCommand(
    Guid LoanId,
    decimal ApprovedAmount,
    decimal InterestRate,
    decimal Fees
) : IRequest<ApiResponse<bool>>;

}
