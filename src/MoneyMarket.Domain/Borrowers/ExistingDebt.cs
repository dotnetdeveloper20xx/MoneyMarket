namespace MoneyMarket.Domain.Borrowers
{
    public sealed class ExistingDebt
    {
        public Guid Id { get; private set; }
        public string LenderName { get; private set; } = default!;
        public string DebtType { get; private set; } = default!; // Credit Card, Car Loan, etc.
        public decimal Amount { get; private set; }
        private ExistingDebt() { }
        public ExistingDebt(string lender, string type, decimal amount)
        { Id = Guid.NewGuid(); LenderName = lender; DebtType = type; Amount = amount; }
    }
}
