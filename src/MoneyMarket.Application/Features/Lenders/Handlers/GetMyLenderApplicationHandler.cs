using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Application.Features.Lenders.Queries;

namespace MoneyMarket.Application.Features.Lenders.Handlers;

public sealed class GetMyLenderApplicationHandler
    : IRequestHandler<GetMyLenderApplicationQuery, LenderApplicationSummaryDto?>
{
    private readonly ILenderApplicationRepository _repo;
    private readonly ICurrentUserService _current;

    public GetMyLenderApplicationHandler(ILenderApplicationRepository repo, ICurrentUserService current)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        _current = current ?? throw new ArgumentNullException(nameof(current));
    }

    public async Task<LenderApplicationSummaryDto?> Handle(GetMyLenderApplicationQuery request, CancellationToken ct)
    {
        if (!Guid.TryParse(_current.UserId, out var userId))
            throw new InvalidOperationException("Invalid user id in token.");

        var app = await _repo.GetMineAsync(userId, asNoTracking: true, ct);
        if (app is null) return null;

        return new LenderApplicationSummaryDto(
            app.LenderApplicationId,
            app.Status,
            app.Email,
            app.CreatedAtUtc,
            app.UpdatedAtUtc
        );
    }
}
