namespace MoneyMarket.Application.Features.Lenders.Dtos
{
    public sealed record UpsertRiskManagementDto(
     string UnderwritingPolicy,
     List<string> RiskAssessmentTools,
     string PaymentCollectionProcess,
     string CommunicationPlan,
     string DefaultHandlingStrategy,
     string PricingStrategy);
}
