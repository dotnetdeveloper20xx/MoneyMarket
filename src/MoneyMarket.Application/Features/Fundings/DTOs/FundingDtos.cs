namespace MoneyMarket.Application.Features.Fundings.DTOs
{
    public sealed record FundingSummaryDto(Guid FundingId, Guid LoanId, decimal Amount, DateTime CreatedAtUtc);
}
