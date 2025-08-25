using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Fundings.Commands.FundLoan;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Enums;

public sealed class FundLoanHandler : IRequestHandler<FundLoanCommand, ApiResponse<bool>>
{
    private readonly IAppDbContext _db;
    public FundLoanHandler(IAppDbContext db) => _db = db;

    public async Task<ApiResponse<bool>> Handle(FundLoanCommand req, CancellationToken ct)
    {
        var loan = await _db.Loans
            .Include(l => l.Fundings)
            .FirstOrDefaultAsync(l => l.LoanId == req.LoanId, ct);

        if (loan is null)
            return ApiResponse<bool>.Fail("Loan not found");

        // open to fund only when PendingFunding
        if (loan.Status != LoanStatus.PendingFunding)
            return ApiResponse<bool>.Fail("Loan is not open for funding", "not_open");

        var target = (loan.ApprovedAmount > 0 ? loan.ApprovedAmount : loan.RequestedAmount);
        var fundedSoFar = loan.Fundings.Sum(f => f.Amount);
        var remaining = target - fundedSoFar;

        if (req.Amount <= 0)
            return ApiResponse<bool>.Fail("Amount must be > 0", "amount_invalid");

        if (req.Amount > remaining)
            return ApiResponse<bool>.Fail("Amount exceeds remaining target", "overfund");

        var funding = new Funding
        {
            Id = Guid.NewGuid(),
            LoanId = loan.LoanId,
            LenderId = req.LenderId,
            Amount = req.Amount,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Fundings.Add(funding);

        await _db.SaveChangesAsync(ct);
        return ApiResponse<bool>.SuccessResult(true, "Loan funded");
    }
}
