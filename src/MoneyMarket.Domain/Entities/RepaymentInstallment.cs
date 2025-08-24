using MoneyMarket.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace MoneyMarket.Domain.Entities;

public class RepaymentInstallment : BaseEntity
{    
    public Guid InstallmentId { get; private set; } = Guid.NewGuid();
    public Guid LoanId { get; private set; }
    public int Sequence { get; private set; }
    public decimal PrincipalAmount { get; private set; }
    public decimal InterestAmount { get; private set; }
    public decimal TotalDue => PrincipalAmount + InterestAmount;
    public DateTime DueDateUtc { get; private set; }
    public bool IsPaid { get; private set; }
    public DateTime? PaidAtUtc { get; private set; }

    private RepaymentInstallment() { }

    public RepaymentInstallment(Guid loanId, int sequence, decimal principal, decimal interest, DateTime dueDateUtc)
    {
        if (sequence <= 0) throw new ArgumentOutOfRangeException(nameof(sequence));
        if (principal < 0 || interest < 0) throw new ArgumentOutOfRangeException();

        LoanId = loanId;
        Sequence = sequence;
        PrincipalAmount = decimal.Round(principal, 2);
        InterestAmount = decimal.Round(interest, 2);
        DueDateUtc = dueDateUtc;
    }

    public void Pay(decimal amount)
    {
        if (IsPaid) throw new InvalidOperationException("Installment already paid.");
        if (amount < TotalDue) throw new InvalidOperationException("Partial payments not supported (domain rule).");
        IsPaid = true;
        PaidAtUtc = DateTime.UtcNow;
    }
}
