using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class UploadBorrowerDocumentHandler
     : IRequestHandler<UploadBorrowerDocumentCommand, ApiResponse<string>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _user;
        private readonly IDateTime _clock;
        private readonly IFileStorage _storage;

        public UploadBorrowerDocumentHandler(IBorrowerRepository repo, ICurrentUserService user, IDateTime clock, IFileStorage storage)
        { _repo = repo; _user = user; _clock = clock; _storage = storage; }

        public async Task<ApiResponse<string>> Handle(UploadBorrowerDocumentCommand request, CancellationToken ct)
        {
            var uid = _user.UserId!;
            var profile = await _repo.GetByUserIdAsync(uid, false, ct) ?? throw new InvalidOperationException("Profile not found.");

            var ext = Path.GetExtension(request.File.FileName);
            var path = $"borrowers/{uid}/{request.File.Type.ToString().ToLowerInvariant()}{ext}";
            var stored = await _storage.UploadAsync("docs", path, request.File.Content, request.File.ContentType, ct);

            var doc = new BorrowerDocument(request.File.Type, request.File.FileName, stored, _clock.UtcNow);
            profile.AddOrReplaceDocument(doc, _clock.UtcNow);
            _repo.Update(profile);

            return ApiResponse<string>.SuccessResult(stored, "Document uploaded.");
        }
    }
}
