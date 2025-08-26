using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Domain.Borrowers.Events;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class SubmitProfileHandler
    : IRequestHandler<SubmitProfileCommand, ApiResponse<bool>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _user;
        private readonly IDateTime _clock;
        private readonly IMediator _mediator;

        public SubmitProfileHandler(IBorrowerRepository repo, ICurrentUserService user, IDateTime clock, IMediator mediator)
        { _repo = repo; _user = user; _clock = clock; _mediator = mediator; }

        public async Task<ApiResponse<bool>> Handle(SubmitProfileCommand request, CancellationToken ct)
        {
            var uid = _user.UserId!;
            var profile = await _repo.GetByUserIdAsync(uid, false, ct) ?? throw new InvalidOperationException("Profile not found.");
            profile.Submit(_clock.UtcNow, uid);
            _repo.Update(profile);

            await _mediator.Publish(new ProfileSubmittedEvent(profile.Id, profile.UserId, profile.Email), ct);
            return ApiResponse<bool>.SuccessResult(true, "Profile submitted.");
        }
    }
}
