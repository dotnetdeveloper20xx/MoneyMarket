namespace MoneyMarket.Application.Features.Loans.Dtos
{
    public sealed record LoanSummaryDto(Guid LoanId, string Title, decimal TargetAmount, decimal FundedAmount, decimal Apr, string Status);
}
