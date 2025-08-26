using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands;
using MoneyMarket.Application.Features.Lenders.Dtos;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers
{
    public sealed class UpsertLenderBusinessHandler
     : IRequestHandler<UpsertLenderBusinessCommand, LenderApplicationSummaryDto>
    {
        private readonly ILenderApplicationRepository _repo;
        private readonly ICurrentUserService _current;

        public UpsertLenderBusinessHandler(ILenderApplicationRepository repo, ICurrentUserService current)
            => (_repo, _current) = (repo, current);

        public async Task<LenderApplicationSummaryDto> Handle(UpsertLenderBusinessCommand request, CancellationToken ct)
        {
            if (!Guid.TryParse(_current.UserId, out var userId))
                throw new InvalidOperationException("Invalid user id in token.");
            var email = _current.Email!;
            var app = await _repo.GetMineAsync(userId, asNoTracking: false, ct)
                      ?? LenderApplication.Start(userId, email);

            var dto = request.Dto;
            var info = new BusinessRegistrationInfo(
                dto.BusinessName, dto.RegistrationNumber,
                dto.ProofOfIncorporationDocuments, dto.LendingLicenses, dto.ComplianceStatement);

            app.UpsertBusinessRegistration(info, email);
            if (app.CreatedAtUtc == default) await _repo.AddAsync(app, ct);
            else _repo.Update(app);

            await _repo.SaveChangesAsync(ct);
            return new LenderApplicationSummaryDto(app.LenderApplicationId, app.Status, app.Email, app.CreatedAtUtc, app.UpdatedAtUtc);
        }
    }
}
