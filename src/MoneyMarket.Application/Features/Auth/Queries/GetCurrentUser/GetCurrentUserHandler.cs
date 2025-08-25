using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Auth.Dtos;
using MoneyMarket.Application.Features.Auth.Queries.GetCurrentUser;

public sealed class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, MeDto>
{
    private readonly ICurrentUserService _currentUser;

    public GetCurrentUserHandler(ICurrentUserService currentUser)
        => _currentUser = currentUser;

    public Task<MeDto> Handle(GetCurrentUserQuery _, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated)
            throw new UnauthorizedAccessException("Not authenticated.");

        var dto = new MeDto(
                    UserId: _currentUser.UserId ?? string.Empty,
                    Email: _currentUser.Email ?? string.Empty,
                    Roles: _currentUser.Roles
);

        return Task.FromResult(dto);
    }
}
