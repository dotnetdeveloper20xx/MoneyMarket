using AutoMapper;
using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class UpsertPersonalInfoHandler
    : IRequestHandler<UpsertPersonalInfoCommand, ApiResponse<bool>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _clock;
        private readonly IMapper _mapper;

        public UpsertPersonalInfoHandler(
            IBorrowerRepository repo,
            ICurrentUserService currentUser,
            IDateTime clock,
            IMapper mapper)
        {
            _repo = repo; _currentUser = currentUser; _clock = clock; _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> Handle(UpsertPersonalInfoCommand request, CancellationToken ct)
        {
            var uid = _currentUser.GetRequiredUserIdGuid();
            var existing = await _repo.GetByUserIdAsync(uid.ToString(), asNoTracking: false, ct);

            var address = _mapper.Map<Address>(request.Data.Address);

            if (existing is null)
            {
                var profile = BorrowerProfile.CreateDraft(
                    uid,
                    request.Data.FirstName,
                    request.Data.LastName,
                    request.Data.DateOfBirth,
                    request.Data.NationalIdNumber,
                    address,
                    request.Data.PhoneNumber,
                    request.Data.Email
                );

                await _repo.AddAsync(profile, ct);
            }
            else
            {
                existing.UpdatePersonal(
                    request.Data.FirstName,
                    request.Data.LastName,
                    request.Data.DateOfBirth,
                    request.Data.NationalIdNumber,
                    address,
                    request.Data.PhoneNumber,
                    request.Data.Email,
                    _clock.UtcNow
                );

                _repo.Update(existing);
            }

            // No SaveChanges here: UnitOfWorkBehavior handles it
            return ApiResponse<bool>.SuccessResult(true, "Personal information saved.");
        }
    }
}
