namespace MoneyMarket.Application.Features.Lenders.Dtos
{
    public sealed record UpsertFinancialCapacityDto(
     string FundingSourceType,
     string FundingSourceDescription,
     List<string> CapitalReserveDocuments);
}
