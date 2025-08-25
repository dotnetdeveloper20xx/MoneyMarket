using MediatR;
using MoneyMarket.Application.Features.Auth.Dtos;

namespace MoneyMarket.Application.Features.Auth.Queries.GetCurrentUser
{
    public sealed record GetCurrentUserQuery() : IRequest<MeDto>;
}
