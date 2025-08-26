namespace MoneyMarket.Application.Features.CRM.Dtos
{
    public sealed record LenderRowDto(Guid Id, string Email, string? DisplayName, bool IsDisabled);
}
