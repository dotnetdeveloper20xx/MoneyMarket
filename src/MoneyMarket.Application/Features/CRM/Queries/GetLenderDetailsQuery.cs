using MediatR;
using MoneyMarket.Application.Features.CRM.Dtos;

namespace MoneyMarket.Application.Features.CRM.Queries
{
    public sealed record GetLenderDetailsQuery(Guid LenderId) : IRequest<LenderDetailsDto?>;
}
