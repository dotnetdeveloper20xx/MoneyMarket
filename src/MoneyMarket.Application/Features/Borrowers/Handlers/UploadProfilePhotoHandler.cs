using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class UploadProfilePhotoHandler
     : IRequestHandler<UploadProfilePhotoCommand, ApiResponse<string>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _user;
        private readonly IDateTime _clock;
        private readonly IFileStorage _storage;

        public UploadProfilePhotoHandler(IBorrowerRepository repo, ICurrentUserService user, IDateTime clock, IFileStorage storage)
        { _repo = repo; _user = user; _clock = clock; _storage = storage; }

        public async Task<ApiResponse<string>> Handle(UploadProfilePhotoCommand request, CancellationToken ct)
        {
            var uid = _user.UserId!;
            var profile = await _repo.GetByUserIdAsync(uid, false, ct) ?? throw new InvalidOperationException("Profile not found.");

            var ext = Path.GetExtension(request.File.FileName);
            var path = $"profiles/{uid}/photo{ext}";
            var stored = await _storage.UploadAsync("images", path, request.File.Content, request.File.ContentType, ct);
            profile.SetPhoto(stored, _clock.UtcNow);
            _repo.Update(profile);

            return ApiResponse<string>.SuccessResult(stored, "Photo uploaded.");
        }
    }
}
