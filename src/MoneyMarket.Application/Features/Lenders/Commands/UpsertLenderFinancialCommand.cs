using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    // Upsert Financial
    public sealed record UpsertLenderFinancialCommand(UpsertFinancialCapacityDto Dto) : IRequest<LenderApplicationSummaryDto>, ITransactionalRequest;

}
