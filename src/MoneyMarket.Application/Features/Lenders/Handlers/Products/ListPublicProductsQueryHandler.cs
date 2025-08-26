using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;
using MoneyMarket.Application.Features.Lenders.Mappings;
using MoneyMarket.Application.Features.Lenders.Queries.Products;

namespace MoneyMarket.Application.Features.Lenders.Handlers.Products
{
    public sealed class ListPublicProductsQueryHandler
         : IRequestHandler<ListPublicProductsQuery, PagedResult<LenderProductViewDto>>
    {
        private readonly ILenderProductRepository _products;

        public ListPublicProductsQueryHandler(ILenderProductRepository products) => _products = products;

        public async Task<PagedResult<LenderProductViewDto>> Handle(ListPublicProductsQuery request, CancellationToken ct)
        {
            var (items, total) = await _products.GetPublicPagedAsync(request.PageNumber, request.PageSize, ct);
            var mapped = items.Select(p => p.ToView()).ToList();

            return PagedResult<LenderProductViewDto>.Create(mapped, request.PageNumber, request.PageSize, total);
        }
    }
}
