namespace MoneyMarket.Api.Contracts.Fundings
{
    /// <summary>
    /// Transport DTO for funding a loan.
    /// </summary>
    public sealed record FundLoanRequest(decimal Amount);
}
