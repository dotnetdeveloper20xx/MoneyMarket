using MediatR;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    // Upsert Business
    public sealed record UpsertLenderBusinessCommand(UpsertBusinessRegistrationDto Dto) : IRequest<LenderApplicationSummaryDto>;

}
