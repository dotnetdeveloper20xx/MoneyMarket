namespace MoneyMarket.Application.Features.Lenders.Dtos
{
    public sealed record LenderProfileDto(
       Guid LenderId,
       Guid UserId,
       string Email,
       string BusinessName,
       string RegistrationNumber,
       string ComplianceStatement,
       string? PhotoPath,         
       decimal TotalFunded,
       decimal TotalProfit,
       int LoansFundedCount,
       bool IsDisabled,
       string? DisabledReason,
       DateTime? DisabledAtUtc,
       DateTime CreatedAtUtc,
       DateTime? UpdatedAtUtc
   );
}
