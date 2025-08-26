using MediatR;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;

namespace MoneyMarket.Application.Features.Lenders.Queries.Products
{
    public sealed record ListMyProductsQuery(int PageNumber = 1, int PageSize = 20) : IRequest<PagedResult<LenderProductViewDto>>;
}
