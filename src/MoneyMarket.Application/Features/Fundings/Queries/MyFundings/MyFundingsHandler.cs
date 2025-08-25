using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Fundings.DTOs;

namespace MoneyMarket.Application.Features.Fundings.Queries.MyFundings
{
    public sealed class MyFundingsHandler : IRequestHandler<MyFundingsQuery, IReadOnlyList<FundingSummaryDto>>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IFundingRepository _fundings;

        public MyFundingsHandler(ICurrentUserService currentUser, IFundingRepository fundings)
        {
            _currentUser = currentUser;
            _fundings = fundings;
        }

        public async Task<IReadOnlyList<FundingSummaryDto>> Handle(MyFundingsQuery req, CancellationToken ct)
        {
            var userIdStr = _currentUser.UserId;
            if (string.IsNullOrWhiteSpace(userIdStr) || !Guid.TryParse(userIdStr, out var lenderId))
                return new List<FundingSummaryDto>(); // or throw UnauthorizedAccessException and map via ProblemDetails

            return await _fundings.GetSummariesByLenderAsync(lenderId, ct);
        }
    }
}
