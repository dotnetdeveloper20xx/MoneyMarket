using MediatR;

namespace MoneyMarket.Application.Features.Lenders.Commands.Products
{
    public sealed record DeactivateProductCommand(Guid ProductId) : IRequest<bool>;
}
