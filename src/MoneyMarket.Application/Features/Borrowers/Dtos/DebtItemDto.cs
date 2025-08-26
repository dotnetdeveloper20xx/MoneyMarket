namespace MoneyMarket.Application.Features.Borrowers.Dtos
{
    public sealed record DebtItemDto(
    string LenderName,
    string DebtType,
    decimal Amount
);
}
