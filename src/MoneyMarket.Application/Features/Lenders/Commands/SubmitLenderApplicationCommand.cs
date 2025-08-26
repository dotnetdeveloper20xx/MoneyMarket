using MediatR;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    // Submit Application
    public sealed record SubmitLenderApplicationCommand() : IRequest<LenderApplicationSummaryDto>;

}
