using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Loans.Commands.SubmitLoan;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Enums;

public sealed class SubmitLoanHandler : IRequestHandler<SubmitLoanCommand, ApiResponse<Guid>>
{
    private readonly IAppDbContext _db;
    public SubmitLoanHandler(IAppDbContext db) => _db = db;

    public async Task<ApiResponse<Guid>> Handle(SubmitLoanCommand req, CancellationToken ct)
    {
        var borrower = await _db.Borrowers
            .FirstOrDefaultAsync(b => b.BorrowerId == req.BorrowerId, ct);

        if (borrower is null)
            return ApiResponse<Guid>.Fail("Borrower not found", "borrower_missing");

        var loan = new Loan(req.BorrowerId, req.TargetAmount, req.Description, 12, RepaymentFrequency.Monthly);

        _db.Loans.Add(loan);
        await _db.SaveChangesAsync(ct);

        return ApiResponse<Guid>.SuccessResult(loan.LoanId, "Loan submitted");
    }
}
