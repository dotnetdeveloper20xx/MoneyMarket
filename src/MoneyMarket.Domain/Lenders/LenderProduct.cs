using MoneyMarket.Domain.Common;

namespace MoneyMarket.Domain.Lenders
{
    public sealed class LenderProduct : AuditableEntity
    {
        public Guid LenderProductId { get; private set; } = Guid.NewGuid();
        public Guid LenderId { get; private set; }

        public string Name { get; private set; } = string.Empty;             // "Short Term Loan"
        public LenderProductTermType TermType { get; private set; }
        public decimal MinAmount { get; private set; }
        public decimal MaxAmount { get; private set; }
        public int TermMonths { get; private set; }
        public int Instalments { get; private set; }

        /// <summary>Total annualized interest rate offered to borrower (e.g., 0.10 = 10%).</summary>
        public decimal InterestRate { get; private set; }

        /// <summary>Platform cut is fixed 3% (0.03). Lender must leave at least 2% margin if min is 5%.</summary>
        public const decimal PlatformShare = 0.03m;
        public const decimal MinInterestRate = 0.05m;

        public bool IsActive { get; private set; } = true;

        private LenderProduct() { }

        public static LenderProduct Create(
            Guid lenderId,
            string name,
            LenderProductTermType type,
            decimal minAmount,
            decimal maxAmount,
            int termMonths,
            int instalments,
            decimal interestRate,
            string actor)
        {
            if (lenderId == Guid.Empty) throw new ArgumentException(nameof(lenderId));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (minAmount <= 0 || maxAmount <= 0 || maxAmount < minAmount) throw new ArgumentOutOfRangeException(nameof(maxAmount));
            if (termMonths <= 0 || instalments <= 0) throw new ArgumentOutOfRangeException(nameof(termMonths));
            if (interestRate < MinInterestRate) throw new ArgumentOutOfRangeException(nameof(interestRate), $"Interest must be >= {MinInterestRate:P0}");
            if (interestRate <= PlatformShare) throw new ArgumentException("Interest must exceed platform share (3%).");

            return new LenderProduct
            {
                LenderId = lenderId,
                Name = name.Trim(),
                TermType = type,
                MinAmount = decimal.Round(minAmount, 2),
                MaxAmount = decimal.Round(maxAmount, 2),
                TermMonths = termMonths,
                Instalments = instalments,
                InterestRate = decimal.Round(interestRate, 4)
            }.Touched(actor);
        }

        public decimal LenderMargin() => InterestRate - PlatformShare; // must be >= 0.02 if min rate is 5%

        public void UpdateTerms(decimal minAmt, decimal maxAmt, int termMonths, int instalments, decimal rate, string actor)
        {
            if (minAmt <= 0 || maxAmt <= 0 || maxAmt < minAmt) throw new ArgumentOutOfRangeException();
            if (termMonths <= 0 || instalments <= 0) throw new ArgumentOutOfRangeException();
            if (rate < MinInterestRate || rate <= PlatformShare) throw new ArgumentOutOfRangeException(nameof(rate));

            MinAmount = decimal.Round(minAmt, 2);
            MaxAmount = decimal.Round(maxAmt, 2);
            TermMonths = termMonths;
            Instalments = instalments;
            InterestRate = decimal.Round(rate, 4);
            Touched(actor);
        }

        public void Deactivate(string actor) { IsActive = false; Touched(actor); }

        private LenderProduct Touched(string actor) { Touch(actor); return this; }
    }
}
