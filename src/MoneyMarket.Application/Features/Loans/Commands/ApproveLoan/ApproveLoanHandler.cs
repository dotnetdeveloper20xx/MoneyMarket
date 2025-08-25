using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Loans.Commands.ApproveLoan;

public sealed class ApproveLoanHandler : IRequestHandler<ApproveLoanCommand, ApiResponse<bool>>
{
    private readonly IAppDbContext _db;
    public ApproveLoanHandler(IAppDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(ApproveLoanCommand req, CancellationToken ct)
    {
        var loan = await _db.Loans
            .FirstOrDefaultAsync(l => l.LoanId == req.LoanId, ct);

        if (loan is null)
            return ApiResponse<bool>.Fail("Loan not found", "loan_missing");

        loan.Approve(req.ApprovedAmount, req.InterestRate, req.Fees);

        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.SuccessResult(true, "Loan approved and opened for funding");
    }
}
