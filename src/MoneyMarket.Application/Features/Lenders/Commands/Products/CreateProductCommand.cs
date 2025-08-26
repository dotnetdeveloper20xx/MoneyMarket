using MediatR;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;

namespace MoneyMarket.Application.Features.Lenders.Commands.Products
{
    public sealed record CreateProductCommand(CreateProductDto Dto) : IRequest<LenderProductViewDto>;
}
