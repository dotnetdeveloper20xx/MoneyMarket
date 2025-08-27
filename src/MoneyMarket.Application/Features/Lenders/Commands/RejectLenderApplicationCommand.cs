using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    public sealed record RejectLenderApplicationCommand(Guid ApplicationId, string Reason)
           : IRequest<LenderApplicationSummaryDto>, ITransactionalRequest;
}
