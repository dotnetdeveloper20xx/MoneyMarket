using AutoMapper;
using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Commands;
using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class UpsertFinancialInfoHandler
     : IRequestHandler<UpsertFinancialInfoCommand, ApiResponse<bool>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IDateTime _clock;
        private readonly IMapper _mapper;

        public UpsertFinancialInfoHandler(
            IBorrowerRepository repo,
            ICurrentUserService currentUser,
            IDateTime clock,
            IMapper mapper)
        { _repo = repo; _currentUser = currentUser; _clock = clock; _mapper = mapper; }

        public async Task<ApiResponse<bool>> Handle(UpsertFinancialInfoCommand request, CancellationToken ct)
        {
            var uid = _currentUser.UserId ?? throw new InvalidOperationException("User not found in context.");
            var profile = await _repo.GetByUserIdAsync(uid, asNoTracking: false, ct)
                ?? throw new InvalidOperationException("Create personal info first.");

            var emp = _mapper.Map<EmploymentInfo>(request.Data);
            profile.UpsertEmployment(emp, _clock.UtcNow);

            _repo.Update(profile);
            return ApiResponse<bool>.SuccessResult(true, "Financial information saved.");
        }
    }
}
