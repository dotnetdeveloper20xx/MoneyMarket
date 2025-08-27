using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Exceptions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class CreateLenderBusinessHandler
        : IRequestHandler<CreateLenderBusinessCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _repo;
        private readonly ICurrentUserService _current;

        public CreateLenderBusinessHandler(ILenderApplicationRepository repo, ICurrentUserService current)
            => (_repo, _current) = (repo, current);

        public async Task<LenderApplicationSummaryDto> Handle(CreateLenderBusinessCommand request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");

            var email = _current.Email ?? throw new InvalidOperationException("Missing email in token.");

            // Ensure the user doesn't already have an application
            var exists = await _repo.ExistsForUserAsync(userId, ct);
            if (exists)
                throw new ConflictException("An application already exists for this user.");

            var dto = request.Dto;
            var info = new BusinessRegistrationInfo(
                dto.BusinessName, dto.RegistrationNumber,
                dto.ProofOfIncorporationDocuments, dto.LendingLicenses, dto.ComplianceStatement);

            var app = LenderApplication.Start(userId, email);
            app.UpsertBusinessRegistration(info, email);

            await _repo.AddAsync(app, ct); // tracked insert; UoW will SaveChanges

            return new LenderApplicationSummaryDto(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
