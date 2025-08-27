using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    public sealed record ApproveLenderApplicationCommand(Guid ApplicationId)
          : IRequest<LenderApplicationSummaryDto>, ITransactionalRequest;
}
