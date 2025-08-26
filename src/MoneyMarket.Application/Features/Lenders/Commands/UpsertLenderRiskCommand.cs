using MediatR;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    // Upsert Risk
    public sealed record UpsertLenderRiskCommand(UpsertRiskManagementDto Dto) : IRequest<LenderApplicationSummaryDto>;

}
