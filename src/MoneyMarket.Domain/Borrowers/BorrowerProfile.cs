
namespace MoneyMarket.Domain.Borrowers;
public sealed class BorrowerProfile
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public DateTime DateOfBirth { get; private set; }
    public int Age { get; private set; }
    public string NationalIdNumber { get; private set; } = default!;
    public Address CurrentAddress { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public EmploymentInfo? Employment { get; private set; }
    private readonly List<ExistingDebt> _debts = new();
    public IReadOnlyCollection<ExistingDebt> Debts => _debts.AsReadOnly();
    public ProfileStatus Status { get; private set; } = ProfileStatus.Draft;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private BorrowerProfile() { }
    public static BorrowerProfile CreateDraft(
        string userId, string firstName, string lastName, DateTime dob, string nationalId,
        Address address, string phone, string email)
    {
        var profile = new BorrowerProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dob,
            Age = CalcAge(dob, DateTime.UtcNow),
            NationalIdNumber = nationalId,
            CurrentAddress = address,
            PhoneNumber = phone,
            Email = email,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };
        return profile;
    }

    public void UpdatePersonal(string firstName, string lastName, DateTime dob, string nationalId,
        Address address, string phone, string email, DateTime now)
    {
        FirstName = firstName; LastName = lastName; DateOfBirth = dob; Age = CalcAge(dob, now);
        NationalIdNumber = nationalId; CurrentAddress = address; PhoneNumber = phone; Email = email;
        UpdatedAtUtc = now;
    }

    public void UpsertEmployment(EmploymentInfo employment, DateTime now)
    { Employment = employment; UpdatedAtUtc = now; }

    public void ReplaceDebts(IEnumerable<ExistingDebt> debts, DateTime now)
    {
        _debts.Clear();
        _debts.AddRange(debts);
        UpdatedAtUtc = now;
    }

    public void Submit(DateTime now) { Status = ProfileStatus.Submitted; UpdatedAtUtc = now; }

    private static int CalcAge(DateTime dob, DateTime asOf)
    {
        var age = asOf.Year - dob.Year;
        if (dob.Date > asOf.Date.AddYears(-age)) age--;
        return age;
    }
}
