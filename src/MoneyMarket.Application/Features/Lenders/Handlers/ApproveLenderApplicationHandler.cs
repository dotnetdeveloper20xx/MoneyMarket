using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Exceptions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class ApproveLenderApplicationHandler
      : IRequestHandler<ApproveLenderApplicationCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _apps;
        private readonly ILenderProfileRepository _profiles;
        private readonly IIdentityService _identity;
        private readonly ICurrentUserService _current;

        public ApproveLenderApplicationHandler(
            ILenderApplicationRepository apps,
            ILenderProfileRepository profiles,
            IIdentityService identity,
            ICurrentUserService current)
        {
            _apps = apps;
            _profiles = profiles;
            _identity = identity;
            _current = current;
        }

        public async Task<LenderApplicationSummaryDto> Handle(ApproveLenderApplicationCommand request, CancellationToken ct)
        {
            var adminEmail = _current.Email ?? "system";

            var app = await _apps.GetByIdAsync(request.ApplicationId, asNoTracking: false, ct)
                      ?? throw new NotFoundException("Lender application not found.");

            if (app.Status != LenderApplicationStatus.Submitted)
                throw new ConflictException("Only submitted applications can be approved.");

            app.Approve(adminEmail);

            // Provision live Lender
            var lender = Lender.FromApprovedApplication(app, adminEmail);
            await _profiles.AddAsync(lender, ct);

            // Grant Lender role
            await _identity.AddUserToRoleAsync(app.UserId, "Lender", ct);

            return new LenderApplicationSummaryDto(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
