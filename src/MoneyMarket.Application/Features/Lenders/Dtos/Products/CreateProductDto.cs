using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Dtos.Products
{
    public sealed record CreateProductDto(
      string Name,
      LenderProductTermType TermType,
      decimal MinAmount,
      decimal MaxAmount,
      int TermMonths,
      int Instalments,
      decimal InterestRate);
}
