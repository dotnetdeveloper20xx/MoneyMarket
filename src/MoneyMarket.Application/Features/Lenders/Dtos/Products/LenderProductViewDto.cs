using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Dtos.Products
{
    public sealed record LenderProductViewDto(
     Guid LenderProductId,
     string Name,
     LenderProductTermType TermType,
     decimal MinAmount,
     decimal MaxAmount,
     int TermMonths,
     int Instalments,
     decimal InterestRate,
     decimal PlatformShare,
     decimal LenderMargin,
     bool IsActive);
}
