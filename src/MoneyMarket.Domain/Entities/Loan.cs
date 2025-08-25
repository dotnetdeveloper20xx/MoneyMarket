using MoneyMarket.Domain.Common;
using MoneyMarket.Domain.Enums;
using MoneyMarket.Domain.Events;

namespace MoneyMarket.Domain.Entities;

public class Loan : AuditableEntity
{
    // EF Core-friendly key with private setter
    public Guid LoanId { get; private set; } = Guid.NewGuid();

    public Guid BorrowerId { get; private set; }
    public Guid? LenderId { get; private set; }

    public ICollection<Funding> Fundings { get; private set; } = new List<Funding>();

    // Request & terms
    public decimal RequestedAmount { get; private set; }
    public decimal ApprovedAmount { get; private set; }
    public string Purpose { get; private set; } = string.Empty;
    public int TermMonths { get; private set; }
    public RepaymentFrequency RepaymentFrequency { get; private set; }
    public decimal InterestRate { get; private set; }        // % APR (set when approved)
    public decimal Fees { get; private set; }                 // set when approved
    public decimal TotalRepayableAmount { get; private set; } // set when approved

    // Risk
    public int? CreditScoreAtApplication { get; private set; }
    public decimal? DebtToIncomeRatio { get; private set; }
    public string? UnderwritingDecisionNotes { get; private set; }
    public RiskGrade? RiskGrade { get; private set; }

    // Schedule
    private readonly List<RepaymentInstallment> _repaymentSchedule = new();
    public IReadOnlyCollection<RepaymentInstallment> RepaymentSchedule => _repaymentSchedule.AsReadOnly();

    // Status
    public LoanStatus Status { get; private set; } = LoanStatus.Draft;
    public DateTime ApplicationDateUtc { get; private set; } = DateTime.UtcNow;
    public DateTime? FundedAtUtc { get; private set; }

    // EF Core parameterless ctor
    private Loan() { }

 
    public Loan(
        Guid borrowerId,
        decimal requestedAmount,
        string purpose,
        int termMonths,
        RepaymentFrequency frequency,
        DateTime? applicationDateUtc = null)
    {
        if (borrowerId == Guid.Empty) throw new ArgumentException("BorrowerId required.", nameof(borrowerId));
        if (requestedAmount <= 0) throw new ArgumentOutOfRangeException(nameof(requestedAmount), "Must be > 0");
        if (termMonths <= 0) throw new ArgumentOutOfRangeException(nameof(termMonths), "Must be > 0");

        LoanId = Guid.NewGuid();
        BorrowerId = borrowerId;
        RequestedAmount = decimal.Round(requestedAmount, 2);
        Purpose = purpose?.Trim() ?? string.Empty;
        TermMonths = termMonths;
        RepaymentFrequency = frequency;

        Status = LoanStatus.Submitted;                  // initial status on creation
        ApplicationDateUtc = applicationDateUtc ?? DateTime.UtcNow;

        // domain event on submission
        Raise(new LoanSubmittedEvent(LoanId, borrowerId, RequestedAmount));
    }

    public void AttachUnderwriting(int? creditScore, decimal? dti, RiskGrade? risk, string? notes)
    {
        CreditScoreAtApplication = creditScore;
        DebtToIncomeRatio = dti;
        RiskGrade = risk;
        UnderwritingDecisionNotes = notes;
        Touch("underwriter");
    }

    public void Approve(decimal approvedAmount, decimal interestRate, decimal fees)
    {
        if (Status is not LoanStatus.Submitted and not LoanStatus.UnderReview)
            throw new InvalidOperationException("Only submitted or under-review loans can be approved.");

        if (approvedAmount <= 0) throw new ArgumentOutOfRangeException(nameof(approvedAmount));
        if (interestRate < 0 || interestRate > 100) throw new ArgumentOutOfRangeException(nameof(interestRate));
        if (fees < 0) throw new ArgumentOutOfRangeException(nameof(fees));

        ApprovedAmount = decimal.Round(approvedAmount, 2);
        InterestRate = interestRate;
        Fees = decimal.Round(fees, 2);
        TotalRepayableAmount = CalculateTotalRepayable(ApprovedAmount, InterestRate, TermMonths, Fees);

        Status = LoanStatus.PendingFunding;
        Touch("admin-approve");

        GenerateRepaymentSchedule();
        Raise(new LoanApprovedEvent(LoanId, ApprovedAmount, InterestRate, TermMonths));
    }

    public void Decline(string reason)
    {
        if (Status is LoanStatus.Approved or LoanStatus.PendingFunding or LoanStatus.Funded or LoanStatus.Active)
            throw new InvalidOperationException("Cannot decline an approved/funded/active loan.");
        Status = LoanStatus.Declined;
        Touch("admin-decline");
        Raise(new LoanDeclinedEvent(LoanId, reason));
    }

    public void Fund()
    {
        if (Status != LoanStatus.PendingFunding)
            throw new InvalidOperationException("Loan must be pending funding to fund.");

        Status = LoanStatus.Funded;
        FundedAtUtc = DateTime.UtcNow;
        Touch("lender-fund");
        Raise(new LoanFundedEvent(LoanId, ApprovedAmount));
        Activate();
    }

    private void Activate()
    {
        if (Status != LoanStatus.Funded)
            throw new InvalidOperationException("Only funded loans can activate.");
        Status = LoanStatus.Active;
        Touch("system-activate");
    }

    public void RecordPayment(Guid installmentId, decimal amount)
    {
        if (Status is not LoanStatus.Active and not LoanStatus.InArrears)
            throw new InvalidOperationException("Payments allowed only for Active/InArrears loans.");

        var inst = _repaymentSchedule.FirstOrDefault(i => i.InstallmentId == installmentId)
                   ?? throw new InvalidOperationException("Installment not found.");

        inst.Pay(amount);
        Touch("borrower-payment");
        Raise(new LoanPaymentMadeEvent(LoanId, installmentId, amount));

        if (_repaymentSchedule.All(i => i.IsPaid))
        {
            Status = LoanStatus.Completed;
            Touch("system-complete");
            Raise(new LoanCompletedEvent(LoanId));
        }
    }

    public void MarkInArrears()
    {
        if (Status == LoanStatus.Active) Status = LoanStatus.InArrears;
    }

    public void DefaultLoan()
    {
        if (Status is not LoanStatus.Active and not LoanStatus.InArrears)
            throw new InvalidOperationException("Only Active/InArrears loans can default.");
        Status = LoanStatus.Defaulted;
        Touch("system-default");
        Raise(new LoanDefaultedEvent(LoanId));
    }

    public void Cancel(string reason)
    {
        if (Status is LoanStatus.Funded or LoanStatus.Active or LoanStatus.Completed)
            throw new InvalidOperationException("Cannot cancel after funding.");
        Status = LoanStatus.Cancelled;
        Touch($"cancel:{reason}");
        Raise(new LoanCancelledEvent(LoanId));
    }

    private decimal CalculateTotalRepayable(decimal principal, decimal annualRatePercent, int months, decimal fees)
    {
        // Simple interest for demo; replace with amortization if needed.
        var interest = principal * (annualRatePercent / 100m) * (months / 12m);
        return decimal.Round(principal + fees + interest, 2);
    }

    private void GenerateRepaymentSchedule()
    {
        _repaymentSchedule.Clear();

        var monthlyInterest = (InterestRate / 100m) / 12m;
        var principalPerInstallment = ApprovedAmount / TermMonths;

        for (int i = 1; i <= TermMonths; i++)
        {
            var interestPortion = ApprovedAmount * monthlyInterest; // naive (non‑amortizing)
            var inst = new RepaymentInstallment(
                LoanId,
                i,
                principalPerInstallment,
                interestPortion,
                DateTime.UtcNow.AddMonths(i));

            _repaymentSchedule.Add(inst);
        }
    }
}
