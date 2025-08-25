using MediatR;
using MoneyMarket.Application.Features.Auth.Dtos;

namespace MoneyMarket.Application.Features.Auth.Queries.GetCurrentUser
{
    public sealed record GetCurrentUserQuery(string UserId, string Email, string[] Roles) : IRequest<MeDto>;
}
