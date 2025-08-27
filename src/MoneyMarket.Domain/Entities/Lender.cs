using MoneyMarket.Domain.Common;
using MoneyMarket.Domain.Lenders; // if you already have LenderApplication + statuses

namespace MoneyMarket.Domain.Entities;

public class Lender : AuditableEntity
{
    public Guid LenderId { get; private set; } = Guid.NewGuid();

    // 🔗 Link to identity user
    public Guid UserId { get; private set; }
    public string Email { get; private set; } = string.Empty;

    // Business & compliance
    public string BusinessName { get; private set; } = string.Empty;
    public string RegistrationNumber { get; private set; } = string.Empty;
    public string ComplianceStatement { get; private set; } = string.Empty;

    // 🔸 New: profile photo
    public string? PhotoPath { get; private set; } // relative or URL

    // Portfolio stats (derived or denormalized)
    public decimal TotalFunded { get; private set; }
    public decimal TotalProfit { get; private set; }
    public int LoansFundedCount { get; private set; }

    // CRM actions
    public bool IsDisabled { get; private set; }
    public string? DisabledReason { get; private set; }
    public DateTime? DisabledAtUtc { get; private set; }

    private Lender() { }

    public static Lender Register(Guid userId, string businessName, string registrationNumber, string complianceStatement, string? email = null)
    {
        if (userId == Guid.Empty) throw new ArgumentException("UserId required.");
        if (string.IsNullOrWhiteSpace(businessName)) throw new ArgumentException("Business name is required.");
        if (string.IsNullOrWhiteSpace(registrationNumber)) throw new ArgumentException("Registration number is required.");

        return new Lender
        {
            UserId = userId,
            Email = (email ?? string.Empty).Trim(),
            BusinessName = businessName.Trim(),
            RegistrationNumber = registrationNumber.Trim(),
            ComplianceStatement = complianceStatement?.Trim() ?? string.Empty
        };
    }

    /// <summary>Provision a live Lender profile from an approved application.</summary>
    public static Lender FromApprovedApplication(LenderApplication app, string actor)
    {
        if (app.Status != LenderApplicationStatus.Approved)
            throw new InvalidOperationException("Application must be Approved to create a Lender profile.");

        var br = app.BusinessRegistration ?? throw new InvalidOperationException("Missing business registration on application.");

        var lender = Register(app.UserId, br.BusinessName, br.RegistrationNumber, br.ComplianceStatement, app.Email);
        lender.Touch(actor);
        return lender;
    }

    public void SetPhotoPath(string? photoPath, string actor)
    {
        PhotoPath = string.IsNullOrWhiteSpace(photoPath) ? null : photoPath.Trim();
        Touch(actor);
    }

    public void RecordFunding(decimal amount, decimal platformShare = 0m, decimal lenderShare = 0m)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        LoansFundedCount++;
        TotalFunded += decimal.Round(amount, 2);
        TotalProfit += decimal.Round(lenderShare, 2);
        Touch("system");
    }

    public void Disable(string? reason)
    {
        if (IsDisabled) return;
        IsDisabled = true;
        DisabledReason = reason;
        DisabledAtUtc = DateTime.UtcNow;
    }

    public void Enable()
    {
        if (!IsDisabled) return;
        IsDisabled = false;
        DisabledReason = null;
        DisabledAtUtc = null;
    }
}
