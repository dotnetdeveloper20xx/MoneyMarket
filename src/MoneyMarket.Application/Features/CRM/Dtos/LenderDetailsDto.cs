namespace MoneyMarket.Application.Features.CRM.Dtos
{
    public sealed record LenderDetailsDto(Guid Id, string Email, string? DisplayName, 
              bool IsDisabled,  string? DisabledReason, DateTime? DisabledAtUtc);
}
