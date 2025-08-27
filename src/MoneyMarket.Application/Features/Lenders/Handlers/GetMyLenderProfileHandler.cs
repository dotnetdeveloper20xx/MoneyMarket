using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Application.Features.Lenders.Queries;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class GetMyLenderProfileHandler
    : IRequestHandler<GetMyLenderProfileQuery, LenderProfileDto?>
    {
        private readonly ILenderProfileRepository _profiles;
        private readonly ICurrentUserService _current;

        public GetMyLenderProfileHandler(ILenderProfileRepository profiles, ICurrentUserService current)
        {
            _profiles = profiles;
            _current = current;
        }

        public async Task<LenderProfileDto?> Handle(GetMyLenderProfileQuery request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");

            var lender = await _profiles.GetByUserIdAsync(userId, asNoTracking: true, ct);
            if (lender is null) return null;

            return new LenderProfileDto(
                        lender.LenderId,
                        lender.UserId,
                        lender.Email,
                        lender.BusinessName,
                        lender.RegistrationNumber,
                        lender.ComplianceStatement,
                        lender.PhotoPath,               
                        lender.TotalFunded,
                        lender.TotalProfit,
                        lender.LoansFundedCount,
                        lender.IsDisabled,
                        lender.DisabledReason,
                        lender.DisabledAtUtc,
                        lender.CreatedAtUtc,
                        lender.LastModifiedAtUtc
                        );
        }
    }
}
