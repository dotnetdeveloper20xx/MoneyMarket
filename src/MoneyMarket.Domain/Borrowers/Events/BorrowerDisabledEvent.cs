namespace MoneyMarket.Domain.Borrowers.Events
{
    public sealed record BorrowerDisabledEvent(Guid BorrowerId, string BorrowerUserId, string BorrowerEmail);
}
