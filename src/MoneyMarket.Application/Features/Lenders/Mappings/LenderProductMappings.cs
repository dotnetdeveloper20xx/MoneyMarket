using MoneyMarket.Application.Features.Lenders.Dtos.Products;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Mappings
{
    internal static class LenderProductMappings
    {
        public static LenderProductViewDto ToView(this LenderProduct p) =>
            new LenderProductViewDto(
                p.LenderProductId,
                p.Name,
                p.TermType,
                p.MinAmount,
                p.MaxAmount,
                p.TermMonths,
                p.Instalments,
                p.InterestRate,
                LenderProduct.PlatformShare,
                p.LenderMargin(),
                p.IsActive
            );
    }
}
