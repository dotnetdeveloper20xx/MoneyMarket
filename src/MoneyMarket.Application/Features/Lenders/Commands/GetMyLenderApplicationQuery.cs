using MediatR;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    // Get My Application
    public sealed record GetMyLenderApplicationQuery() : IRequest<LenderApplication?>;
}
