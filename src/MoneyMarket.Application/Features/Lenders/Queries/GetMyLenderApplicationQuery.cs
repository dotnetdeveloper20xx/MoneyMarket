using MediatR;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Queries
{
    public sealed record GetMyLenderApplicationQuery
        : IRequest<LenderApplicationSummaryDto?>;
}
