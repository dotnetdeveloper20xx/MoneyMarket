namespace MoneyMarket.Domain.Entities
{
    public class Funding
    {
        public Guid Id { get; set; }
        public Guid LoanId { get; set; }
        public Guid LenderId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAtUtc { get; set; }

        public Loan Loan { get; set; } = null!;
        public Lender Lender { get; set; } = null!;

        public string? IdempotencyKey { get; set; }
    }
}
