using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    public sealed record CreateLenderBusinessCommand(UpsertBusinessRegistrationDto Dto)
     : IRequest<LenderApplicationSummaryDto>, ITransactionalRequest;
}
