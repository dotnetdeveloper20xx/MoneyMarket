namespace MoneyMarket.Api.Contracts.Loans
{
    public sealed record ApproveLoanRequest(
      decimal ApprovedAmount,
      decimal InterestRate,
      decimal Fees);
}
