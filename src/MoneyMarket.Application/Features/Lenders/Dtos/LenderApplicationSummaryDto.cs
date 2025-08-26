using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Dtos
{
    public sealed record LenderApplicationSummaryDto(
     Guid LenderApplicationId,
     LenderApplicationStatus Status,
     string Email,
     DateTime CreatedAtUtc,
     DateTime? UpdatedAtUtc);
}
