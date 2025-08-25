using MediatR;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Loans.Commands.SubmitLoan
{
    public sealed record SubmitLoanCommand(string Title, string Description, decimal TargetAmount, decimal Apr, Guid BorrowerId)
       : IRequest<ApiResponse<Guid>>;
}
