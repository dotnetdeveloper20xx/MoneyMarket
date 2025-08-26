using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.CRM.Dtos;

namespace MoneyMarket.Application.Features.CRM.Queries
{
    public sealed class GetLenderDetailsQueryHandler : IRequestHandler<GetLenderDetailsQuery, LenderDetailsDto?>
    {
        private readonly ILenderProfileRepository _repo;
        public GetLenderDetailsQueryHandler(ILenderProfileRepository repo) => _repo = repo;


        public async Task<LenderDetailsDto?> Handle(GetLenderDetailsQuery request, CancellationToken ct)
        {
            var l = await _repo.GetByIdAsync(request.LenderId, ct);
            if (l is null) return null;
            return new LenderDetailsDto(l.Id, l.Email, l.DisplayName, l.IsDisabled, l.DisabledReason, l.DisabledAtUtc);
        }
    }
}
