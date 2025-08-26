using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.CRM.Dtos;
using MoneyMarket.Application.Features.CRM.Queries;

namespace MoneyMarket.Application.Features.CRM.Handlers
{
    public sealed class GetLenderDetailsQueryHandler : IRequestHandler<GetLenderDetailsQuery, LenderDetailsDto?>
    {
        private readonly ILenderRepository _repo;
        public GetLenderDetailsQueryHandler(ILenderRepository repo) => _repo = repo;


        public async Task<LenderDetailsDto?> Handle(GetLenderDetailsQuery request, CancellationToken ct)
        {
            var l = await _repo.GetByIdAsync(request.LenderId, ct);
            if (l is null) return null;
            return new LenderDetailsDto(l.UserId, l.Email, l.BusinessName, l.IsDisabled, l.DisabledReason, l.DisabledAtUtc);
        }
    }
}
