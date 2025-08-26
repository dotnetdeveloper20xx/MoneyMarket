using MoneyMarket.Domain.Common;

namespace MoneyMarket.Domain.Lenders
{
    public sealed class LenderApplication : AuditableEntity
    {
        public Guid LenderApplicationId { get; private set; } = Guid.NewGuid();
        public Guid UserId { get; private set; }                         // links to Identity
        public string Email { get; private set; } = string.Empty;

        // Step 1 – Legal & Business Registration
        public BusinessRegistrationInfo? BusinessRegistration { get; private set; }

        // Step 2 – Financial Capacity
        public FinancialCapacityInfo? FinancialCapacity { get; private set; }

        // Step 3 – Operational & Risk Management
        public RiskManagementInfo? RiskManagement { get; private set; }

        public LenderApplicationStatus Status { get; private set; } = LenderApplicationStatus.Draft;       
        public DateTime? UpdatedAtUtc { get; private set; }

        private LenderApplication() { }

        public static LenderApplication Start(Guid userId, string email)
        {
            if (userId == Guid.Empty) throw new ArgumentException(nameof(userId));
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException(nameof(email));
            return new LenderApplication { UserId = userId, Email = email.Trim() };
        }

        public void UpsertBusinessRegistration(BusinessRegistrationInfo info, string actor)
        {
            BusinessRegistration = info ?? throw new ArgumentNullException(nameof(info));
            Touch(actor);
        }

        public void UpsertFinancialCapacity(FinancialCapacityInfo info, string actor)
        {
            FinancialCapacity = info ?? throw new ArgumentNullException(nameof(info));
            Touch(actor);
        }

        public void UpsertRiskManagement(RiskManagementInfo info, string actor)
        {
            RiskManagement = info ?? throw new ArgumentNullException(nameof(info));
            Touch(actor);
        }

        public void Submit(string actor)
        {
            if (Status != LenderApplicationStatus.Draft)
                throw new InvalidOperationException("Only Draft applications can be submitted.");
            if (BusinessRegistration is null || FinancialCapacity is null || RiskManagement is null)
                throw new InvalidOperationException("All sections must be completed before submit.");
            Status = LenderApplicationStatus.Submitted;
            Touch(actor);
        }

        public void Approve(string actor)
        {
            if (Status != LenderApplicationStatus.Submitted)
                throw new InvalidOperationException("Only Submitted applications can be approved.");
            Status = LenderApplicationStatus.Approved;
            Touch(actor);
        }

        public void Reject(string reason, string actor)
        {
            if (Status != LenderApplicationStatus.Submitted)
                throw new InvalidOperationException("Only Submitted applications can be rejected.");
            // You may persist reason in an Audit table later
            Status = LenderApplicationStatus.Rejected;
            Touch(actor);
        }       
    }


    public sealed record BusinessRegistrationInfo(
    string BusinessName,
    string RegistrationNumber,
    IReadOnlyList<string> ProofOfIncorporationDocuments,
    IReadOnlyList<string> LendingLicenses,
    string ComplianceStatement);

    public sealed record FinancialCapacityInfo(
        string FundingSourceType,           // enum as string for now
        string FundingSourceDescription,
        IReadOnlyList<string> CapitalReserveDocuments);

    public sealed record RiskManagementInfo(
        string UnderwritingPolicy,
        IReadOnlyList<string> RiskAssessmentTools,
        string PaymentCollectionProcess,
        string CommunicationPlan,
        string DefaultHandlingStrategy,
        string PricingStrategy);
}
