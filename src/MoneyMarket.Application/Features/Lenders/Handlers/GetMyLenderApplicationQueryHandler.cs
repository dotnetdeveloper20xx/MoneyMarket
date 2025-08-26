using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class GetMyLenderApplicationQueryHandler
       : IRequestHandler<GetMyLenderApplicationQuery, LenderApplication?>
    {
        private readonly ILenderApplicationRepository _repo;
        private readonly ICurrentUserService _current;

        public GetMyLenderApplicationQueryHandler(ILenderApplicationRepository repo, ICurrentUserService current)
            => (_repo, _current) = (repo, current);

        public Task<LenderApplication?> Handle(GetMyLenderApplicationQuery request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");
            return _repo.GetMineAsync(userId, asNoTracking: true, ct);
        }
    }
}
