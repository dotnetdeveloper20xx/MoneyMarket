using MediatR;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;

namespace MoneyMarket.Application.Features.Lenders.Commands.Products
{
    public sealed record UpdateProductCommand(Guid ProductId, CreateProductDto Dto) : IRequest<LenderProductViewDto>;
}
