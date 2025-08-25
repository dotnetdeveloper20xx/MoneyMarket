namespace MoneyMarket.Application.Features.Loans.Dtos
{
    public sealed record LoanDetailsDto(Guid LoanId, string Title, string Description, decimal TargetAmount, decimal FundedAmount, decimal Apr, string Status, bool IsVisibleToLenders);
}
