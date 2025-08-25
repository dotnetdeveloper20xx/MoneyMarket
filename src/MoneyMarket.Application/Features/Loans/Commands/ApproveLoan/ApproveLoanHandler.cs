using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Loans.Commands.ApproveLoan;

public sealed class ApproveLoanHandler : IRequestHandler<ApproveLoanCommand, ApiResponse<bool>>
{
    private readonly ILoanRepository _loans;

    public ApproveLoanHandler(ILoanRepository loans) => _loans = loans;

    public async Task<ApiResponse<bool>> Handle(ApproveLoanCommand req, CancellationToken ct)
    {
        var loan = await _loans.GetByIdAsync(req.LoanId, ct);
        if (loan is null)
            return ApiResponse<bool>.Fail("Loan not found", "loan_missing");

        // Domain method (no user id required here unless your entity tracks approver)
        loan.Approve(req.ApprovedAmount, req.InterestRate, req.Fees);

        // No SaveChanges here — UnitOfWorkBehavior will commit for ITransactionalRequest
        return ApiResponse<bool>.SuccessResult(true, "Loan approved and opened for funding");
    }
}
