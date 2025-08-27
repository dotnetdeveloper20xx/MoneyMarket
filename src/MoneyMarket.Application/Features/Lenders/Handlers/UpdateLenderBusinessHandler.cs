using Ardalis.GuardClauses;
using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class UpdateLenderBusinessHandler
       : IRequestHandler<UpdateLenderBusinessCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _repo;
        private readonly ICurrentUserService _current;

        public UpdateLenderBusinessHandler(ILenderApplicationRepository repo, ICurrentUserService current)
            => (_repo, _current) = (repo, current);

        public async Task<LenderApplicationSummaryDto> Handle(UpdateLenderBusinessCommand request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");

            var email = _current.Email ?? throw new InvalidOperationException("Missing email in token.");

            // Load tracked
            var app = await _repo.GetMineAsync(userId, asNoTracking: false, ct)
                      ?? throw new MoneyMarket.Application.Common.Exceptions.NotFoundException("Lender application not found.");

            var dto = request.Dto;
            var info = new BusinessRegistrationInfo(
                dto.BusinessName, dto.RegistrationNumber,
                dto.ProofOfIncorporationDocuments, dto.LendingLicenses, dto.ComplianceStatement);

            // Mutate tracked aggregate; DO NOT call _repo.Update(app)
            app.UpsertBusinessRegistration(info, email);

            // UoW behavior will SaveChanges
            return new LenderApplicationSummaryDto(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
