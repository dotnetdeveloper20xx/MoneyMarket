using MediatR;
using MoneyMarket.Application.Features.Auth.Dtos;

namespace MoneyMarket.Application.Features.Auth.Queries.GetCurrentUser
{
    public sealed class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, MeDto>
    {
        public Task<MeDto> Handle(GetCurrentUserQuery req, CancellationToken ct)
            => Task.FromResult(new MeDto(req.UserId, req.Email, req.Roles));
    }
}
