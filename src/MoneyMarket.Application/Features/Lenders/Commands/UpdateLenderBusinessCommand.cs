using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    public sealed record UpdateLenderBusinessCommand(UpsertBusinessRegistrationDto Dto)
      : IRequest<LenderApplicationSummaryDto>, ITransactionalRequest;
}
