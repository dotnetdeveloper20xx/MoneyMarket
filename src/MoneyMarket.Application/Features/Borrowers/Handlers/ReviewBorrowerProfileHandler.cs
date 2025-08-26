using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class ReviewBorrowerProfileHandler
       : IRequestHandler<ReviewBorrowerProfileCommand, ApiResponse<bool>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _user;
        private readonly IDateTime _clock;

        public ReviewBorrowerProfileHandler(IBorrowerRepository repo, ICurrentUserService user, IDateTime clock)
        { _repo = repo; _user = user; _clock = clock; }

        public async Task<ApiResponse<bool>> Handle(ReviewBorrowerProfileCommand request, CancellationToken ct)
        {
            var adminId = _user.UserId ?? "system";
            var profile = await _repo.GetByIdAsync(request.BorrowerProfileId, false, ct)
                ?? throw new InvalidOperationException("Profile not found.");

            if (request.Approve) profile.Approve(_clock.UtcNow, adminId, request.Reason);
            else profile.Reject(_clock.UtcNow, adminId, request.Reason);

            _repo.Update(profile);
            return ApiResponse<bool>.SuccessResult(true, request.Approve ? "Profile approved." : "Profile rejected.");
        }
    }
}
