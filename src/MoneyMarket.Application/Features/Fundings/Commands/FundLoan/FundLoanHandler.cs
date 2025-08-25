using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Fundings.Commands.FundLoan;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Enums;

public sealed class FundLoanHandler : IRequestHandler<FundLoanCommand, ApiResponse<bool>>
{
    private readonly ICurrentUserService _currentUser;
    private readonly ILoanRepository _loans;
    private readonly IFundingRepository _fundings;
    private readonly IDateTime _clock;
    private readonly IGuidGenerator _guids;

    public FundLoanHandler(
        ICurrentUserService currentUser,
        ILoanRepository loans,
        IFundingRepository fundings,
        IDateTime clock,
        IGuidGenerator guids)
    {
        _currentUser = currentUser;
        _loans = loans;
        _fundings = fundings;
        _clock = clock;
        _guids = guids;
    }

    public async Task<ApiResponse<bool>> Handle(FundLoanCommand req, CancellationToken ct)
    {
        // Auth
        var userIdStr = _currentUser.UserId;
        if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var lenderId))
            return ApiResponse<bool>.Fail("Authentication required", "unauthenticated");

        // Load loan with its fundings (read model)
        var loan = await _loans.GetByIdAsync(req.LoanId, ct);
        if (loan is null)
            return ApiResponse<bool>.Fail("Loan not found", "loan_missing");

        if (loan.Status != LoanStatus.PendingFunding)
            return ApiResponse<bool>.Fail("Loan is not open for funding", "not_open");

        var target = loan.ApprovedAmount > 0 ? loan.ApprovedAmount : loan.RequestedAmount;
        var fundedSoFar = await _fundings.GetTotalFundedForLoanAsync(loan.LoanId, ct);
        var remaining = target - fundedSoFar;

        if (req.Amount > remaining)
            return ApiResponse<bool>.Fail("Amount exceeds remaining target", "overfund");

        // (Optional) Idempotency: only enforce if your table has IdempotencyKey column & unique index
        if (!string.IsNullOrWhiteSpace(req.IdempotencyKey))
        {
            var exists = await _fundings.ExistsByIdempotencyKeyAsync(req.IdempotencyKey!, ct);
            if (exists) return ApiResponse<bool>.SuccessResult(true, "Duplicate request ignored");
        }

        var funding = new Funding
        {
            Id = _guids.NewGuid(),
            LoanId = loan.LoanId,
            LenderId = lenderId,
            Amount = req.Amount,
            CreatedAtUtc = _clock.UtcNow,
            // If your entity has this property, set it:
            // IdempotencyKey = req.IdempotencyKey
        };

        await _fundings.AddAsync(funding, ct);

        // No SaveChanges here — UnitOfWorkBehavior commits
        return ApiResponse<bool>.SuccessResult(true, "Loan funded");
    }
}
