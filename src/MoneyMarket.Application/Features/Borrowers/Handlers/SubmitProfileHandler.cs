using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
        public sealed class SubmitProfileHandler
       : IRequestHandler<SubmitProfileCommand, ApiResponse<bool>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _clock;

        public SubmitProfileHandler(IBorrowerRepository repo, ICurrentUserService currentUser, IDateTime clock)
        { _repo = repo; _currentUser = currentUser; _clock = clock; }

        public async Task<ApiResponse<bool>> Handle(SubmitProfileCommand request, CancellationToken ct)
        {
            var uid = _currentUser.UserId ?? throw new InvalidOperationException("User not found in context.");
            var profile = await _repo.GetByUserIdAsync(uid, asNoTracking: false, ct)
                ?? throw new InvalidOperationException("Profile not found.");

            profile.Submit(_clock.UtcNow);
            _repo.Update(profile);

            return ApiResponse<bool>.SuccessResult(true, "Profile submitted.");
        }
    }
}
