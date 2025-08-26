namespace MoneyMarket.Domain.Borrowers
{
    public sealed class EmploymentInfo
    {
        public string EmployerName { get; private set; } = default!;
        public string JobTitle { get; private set; } = default!;
        public string LengthOfEmployment { get; private set; } = default!; // e.g. "2 years 3 months"
        public decimal GrossAnnualIncome { get; private set; }
        public string? AdditionalIncomeSources { get; private set; }
        private EmploymentInfo() { }
        public EmploymentInfo(string employer, string title, string length, decimal gross, string? additional)
        { EmployerName = employer; JobTitle = title; LengthOfEmployment = length; GrossAnnualIncome = gross; AdditionalIncomeSources = additional; }
    }
}
