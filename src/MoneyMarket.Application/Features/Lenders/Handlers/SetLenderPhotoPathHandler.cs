using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Exceptions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class SetLenderPhotoPathHandler
        : IRequestHandler<SetLenderPhotoPathCommand, LenderProfileDto>
    {
        private readonly ILenderProfileRepository _profiles;
        private readonly ICurrentUserService _current;

        public SetLenderPhotoPathHandler(ILenderProfileRepository profiles, ICurrentUserService current)
        {
            _profiles = profiles;
            _current = current;
        }

        public async Task<LenderProfileDto> Handle(SetLenderPhotoPathCommand request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");
            var email = _current.Email ?? "system";

            // Load tracked
            var lender = await _profiles.GetByUserIdAsync(userId, asNoTracking: false, ct)
                         ?? throw new NotFoundException("Lender profile not found.");

            lender.SetPhotoPath(request.PhotoPath, email);

            // UoW will commit (command implements ITransactionalRequest)

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
