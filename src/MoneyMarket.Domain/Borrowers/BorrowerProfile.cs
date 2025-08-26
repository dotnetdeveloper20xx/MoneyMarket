using System;
using System.Collections.Generic;
using System.Linq;

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

    public string? PhotoPath { get; private set; } // relative or URL

    private readonly List<BorrowerDocument> _documents = new();
    public IReadOnlyCollection<BorrowerDocument> Documents => _documents.AsReadOnly();
    
    private readonly List<AuditTrailEntry> _auditTrail = new();
    public IReadOnlyCollection<AuditTrailEntry> AuditTrail => _auditTrail.AsReadOnly();

    private BorrowerProfile() { }

    public static BorrowerProfile CreateDraft(
        string userId, string firstName, string lastName, DateTime dob, string nationalId,
        Address address, string phone, string email)
    {
        var now = DateTime.UtcNow;
        return new BorrowerProfile
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dob,
            Age = CalcAge(dob, now),
            NationalIdNumber = nationalId,
            CurrentAddress = address,
            PhoneNumber = phone,
            Email = email,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };
    }

    public void UpdatePersonal(string firstName, string lastName, DateTime dob, string nationalId,
        Address address, string phone, string email, DateTime now)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        Age = CalcAge(dob, now);
        NationalIdNumber = nationalId;
        CurrentAddress = address;
        PhoneNumber = phone;
        Email = email;
        UpdatedAtUtc = now;
    }

    public void UpsertEmployment(EmploymentInfo employment, DateTime now)
    {
        Employment = employment;
        UpdatedAtUtc = now;
    }

    public void ReplaceDebts(IEnumerable<ExistingDebt> debts, DateTime now)
    {
        _debts.Clear();
        _debts.AddRange(debts);
        UpdatedAtUtc = now;
    }

      public void Submit(DateTime now)
    {
        Status = ProfileStatus.Submitted;
        UpdatedAtUtc = now;
    }

    private static int CalcAge(DateTime dob, DateTime asOf)
    {
        var age = asOf.Year - dob.Year;
        if (dob.Date > asOf.Date.AddYears(-age)) age--;
        return age;
    }

    public void SetPhoto(string path, DateTime now)
    {
        PhotoPath = path;
        UpdatedAtUtc = now;
    }

    public void AddOrReplaceDocument(BorrowerDocument doc, DateTime now)
    {
        // one doc per type (replace) – adjust if you want multiple payslips
        var existing = _documents.FirstOrDefault(d => d.Type == doc.Type);
        if (existing is not null) _documents.Remove(existing);
        _documents.Add(doc);
        UpdatedAtUtc = now;
    }

    // audited status transitions
    public void Submit(DateTime now, string performedBy)
    {
        var old = Status;
        Status = ProfileStatus.Submitted;
        UpdatedAtUtc = now;
        _auditTrail.Add(new AuditTrailEntry("Submitted", old, Status, null, performedBy, now));
    }

    public void Approve(DateTime now, string performedBy, string? reason = null)
    {
        var old = Status;
        Status = ProfileStatus.Approved;
        UpdatedAtUtc = now;
        _auditTrail.Add(new AuditTrailEntry("Approved", old, Status, reason, performedBy, now));
    }

    public void Reject(DateTime now, string performedBy, string? reason)
    {
        var old = Status;
        Status = ProfileStatus.Rejected;
        UpdatedAtUtc = now;
        _auditTrail.Add(new AuditTrailEntry("Rejected", old, Status, reason, performedBy, now));
    }
}
