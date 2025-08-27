using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Exceptions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class RejectLenderApplicationHandler
         : IRequestHandler<RejectLenderApplicationCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _apps;
        private readonly ICurrentUserService _current;

        public RejectLenderApplicationHandler(ILenderApplicationRepository apps, ICurrentUserService current)
        {
            _apps = apps;
            _current = current;
        }

        public async Task<LenderApplicationSummaryDto> Handle(RejectLenderApplicationCommand request, CancellationToken ct)
        {
            var adminEmail = _current.Email ?? "system";

            var app = await _apps.GetByIdAsync(request.ApplicationId, asNoTracking: false, ct)
                      ?? throw new NotFoundException("Lender application not found.");

            if (app.Status != LenderApplicationStatus.Submitted)
                throw new ConflictException("Only submitted applications can be rejected.");

            app.Reject(request.Reason, adminEmail);

            return new LenderApplicationSummaryDto(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
