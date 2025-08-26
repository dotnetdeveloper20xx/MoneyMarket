using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;
using MoneyMarket.Application.Features.Lenders.Mappings;
using MoneyMarket.Application.Features.Lenders.Queries.Products;

namespace MoneyMarket.Application.Features.Lenders.Handlers.Products
{
    public sealed class ListMyProductsQueryHandler
         : IRequestHandler<ListMyProductsQuery, PagedResult<LenderProductViewDto>>
    {
        private readonly ILenderProductRepository _products;
        private readonly ILenderRepository _lenders;
        private readonly ICurrentUserService _current;

        public ListMyProductsQueryHandler(
            ILenderProductRepository products,
            ILenderRepository lenders,
            ICurrentUserService current)
            => (_products, _lenders, _current) = (products, lenders, current);

        public async Task<PagedResult<LenderProductViewDto>> Handle(ListMyProductsQuery request, CancellationToken ct)
        {
            var lender = await _lenders.GetAggregateByUserIdAsync(_current.UserId, false, ct)
             ?? throw new InvalidOperationException("Lender aggregate not found.");

            if (lender.IsDisabled)
                throw new InvalidOperationException("Your account is on hold and cannot manage products.");

            var lenderId = lender.LenderId;

            var (items, total) = await _products.GetMinePagedAsync(lender.UserId, request.PageNumber, request.PageSize, ct);
            var mapped = items.Select(p => p.ToView()).ToList();

            return PagedResult<LenderProductViewDto>.Create(mapped, request.PageNumber, request.PageSize, total);
        }
    }
}
