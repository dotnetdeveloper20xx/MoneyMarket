using AutoMapper;
using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Dtos;
using MoneyMarket.Application.Features.Borrowers.Queries;
using MoneyMarket.Domain.Borrowers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMarket.Application.Features.Borrowers.Handlers
{
    public sealed class GetMyProfileHandler
     : IRequestHandler<GetMyProfileQuery, ApiResponse<BorrowerProfileViewDto>>
    {
        private readonly IBorrowerRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMapper _mapper;

        public GetMyProfileHandler(IBorrowerRepository repo, ICurrentUserService currentUser, IMapper mapper)
        { _repo = repo; _currentUser = currentUser; _mapper = mapper; }

        public async Task<ApiResponse<BorrowerProfileViewDto>> Handle(GetMyProfileQuery request, CancellationToken ct)
        {
            var uid = _currentUser.UserId ?? throw new InvalidOperationException("User not found in context.");
            var profile = await _repo.GetByUserIdAsync(uid, asNoTracking: true, ct)
                ?? throw new InvalidOperationException("Profile not found.");

            var dto = _mapper.Map<BorrowerProfileViewDto>(profile);
            return ApiResponse<BorrowerProfileViewDto>.SuccessResult(dto);
        }
    }
}
