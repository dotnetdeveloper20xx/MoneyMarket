using MediatR;
using Microsoft.Extensions.Logging;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Exceptions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    /// <summary>
    /// Approves a submitted lender application, provisions the live Lender profile,
    /// and assigns the "Lender" role to the underlying identity.
    /// </summary>
    public sealed class ApproveLenderApplicationHandler
        : IRequestHandler<ApproveLenderApplicationCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _apps;
        private readonly ILenderProfileRepository _profiles;
        private readonly IIdentityService _identity;
        private readonly ICurrentUserService _current;
        private readonly ILogger<ApproveLenderApplicationHandler> _logger;

        public ApproveLenderApplicationHandler(
            ILenderApplicationRepository apps,
            ILenderProfileRepository profiles,
            IIdentityService identity,
            ICurrentUserService current,
            ILogger<ApproveLenderApplicationHandler> logger)
        {
            _apps = apps;
            _profiles = profiles;
            _identity = identity;
            _current = current;
            _logger = logger;
        }

        public async Task<LenderApplicationSummaryDto> Handle(ApproveLenderApplicationCommand request, CancellationToken ct)
        {
            var adminEmail = _current.Email ?? "system";

            var app = await _apps.GetByIdAsync(request.ApplicationId, asNoTracking: false, ct)
                      ?? throw new NotFoundException("Lender application not found.");

            if (app.Status != LenderApplicationStatus.Submitted)
                throw new ConflictException("Only submitted applications can be approved.");

            app.Approve(adminEmail);

            // 1) Resolve canonical Identity Id from email (do NOT trust app.UserId)
            var identityUserId = await _identity.GetUserIdGuidByEmailAsync(app.Email, ct);

            // 2) HARD GUARD: ensure identity row exists (clear error if not)
            await _identity.EnsureUserExistsAsync(identityUserId, ct);

            // 3) Create Lender with VERIFIED id
            var br = app.BusinessRegistration ?? throw new InvalidOperationException("Business registration missing.");
            var lender = Lender.Register(identityUserId, br.BusinessName, br.RegistrationNumber, br.ComplianceStatement, app.Email);
            await _profiles.AddAsync(lender, ct);

            // 4) Assign role to the same verified id
            await _identity.AddUserToRoleAsync(identityUserId, "Lender", ct);

            _logger.LogInformation("Approved AppId={AppId} → LenderId={LenderId}, UserId={UserId}",
                app.LenderApplicationId, lender.LenderId, identityUserId);

            // UoW pipeline will commit because the command is transactional
            return new LenderApplicationSummaryDto(
                app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
