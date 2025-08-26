namespace MoneyMarket.Application.Features.CRM.Dtos
{
    public sealed record BorrowerDetailsDto(Guid Id, string Email, bool IsDisabled, string? DisabledReason, DateTime? DisabledAtUtc);
}
