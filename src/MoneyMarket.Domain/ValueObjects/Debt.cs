namespace MoneyMarket.Domain.ValueObjects;

public sealed record Debt(
    string LenderName,
    string DebtType, // e.g., CreditCard, CarLoan, Mortgage
    decimal Amount)
{
    public static Debt Create(string lenderName, string debtType, decimal amount)
    {
        if (string.IsNullOrWhiteSpace(lenderName)) throw new ArgumentException("LenderName is required.");
        if (string.IsNullOrWhiteSpace(debtType)) throw new ArgumentException("DebtType is required.");
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        return new Debt(lenderName.Trim(), debtType.Trim(), decimal.Round(amount, 2));
    }
}
