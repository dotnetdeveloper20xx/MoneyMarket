using MoneyMarket.Domain.Common;
using MoneyMarket.Domain.ValueObjects;

namespace MoneyMarket.Domain.Entities;

public class Borrower : AuditableEntity
{
    public Guid BorrowerId { get; private set; } = Guid.NewGuid();

    // 🔗 Link to identity user
    public Guid UserId { get; private set; }

    // Personal
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public int Age { get; private set; }
    public string NationalIdNumber { get; private set; } = string.Empty;
    public Address Address { get; private set; } = Address.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    // Financial
    public string EmployerName { get; private set; } = string.Empty;
    public string JobTitle { get; private set; } = string.Empty;
    public string LengthOfEmployment { get; private set; } = string.Empty;
    public decimal GrossAnnualIncome { get; private set; }
    public string AdditionalIncomeSources { get; private set; } = string.Empty;

    private readonly List<Debt> _existingDebts = new();
    public IReadOnlyCollection<Debt> ExistingDebts => _existingDebts.AsReadOnly();

    private Borrower() { }

    public static Borrower Register(
        Guid userId,
        string firstName,
        string lastName,
        DateTime dob,
        string nationalId,
        Address address,
        string phoneNumber,
        string email)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId required.");
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
        if (dob > DateTime.UtcNow.AddYears(-18)) throw new InvalidOperationException("Borrower must be 18+.");
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email is required.");

        return new Borrower
        {
            UserId = userId,
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            DateOfBirth = dob,
            Age = (int)((DateTime.UtcNow - dob).TotalDays / 365.25),
            NationalIdNumber = nationalId?.Trim() ?? string.Empty,
            Address = address,
            PhoneNumber = phoneNumber?.Trim() ?? string.Empty,
            Email = email.Trim()
        };
    }

    public void UpdateFinancials(
        string employerName,
        string jobTitle,
        string lengthOfEmployment,
        decimal grossAnnualIncome,
        string additionalIncomeSources)
    {
        EmployerName = employerName?.Trim() ?? "";
        JobTitle = jobTitle?.Trim() ?? "";
        LengthOfEmployment = lengthOfEmployment?.Trim() ?? "";
        GrossAnnualIncome = grossAnnualIncome < 0 ? 0 : decimal.Round(grossAnnualIncome, 2);
        AdditionalIncomeSources = additionalIncomeSources?.Trim() ?? "";
        Touch("system");
    }

    public void SetDebts(IEnumerable<Debt> debts)
    {
        _existingDebts.Clear();
        _existingDebts.AddRange(debts ?? Array.Empty<Debt>());
        Touch("system");
    }
}
