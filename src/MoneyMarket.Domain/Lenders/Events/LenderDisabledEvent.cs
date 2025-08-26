namespace MoneyMarket.Domain.Lenders.Events
{
    public sealed record LenderDisabledEvent(Guid LenderId, string LenderUserId, string LenderEmail);
}
