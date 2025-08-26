using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.CRM.Dtos;

namespace MoneyMarket.Application.Features.CRM.Queries
{
    public sealed class GetBorrowerDetailsQueryHandler : IRequestHandler<GetBorrowerDetailsQuery, BorrowerDetailsDto?>
    {
        private readonly IBorrowerRepository _repo;
        public GetBorrowerDetailsQueryHandler(IBorrowerRepository repo) => _repo = repo;

        public async Task<BorrowerDetailsDto?> Handle(GetBorrowerDetailsQuery request, CancellationToken ct)
        {
            var b = await _repo.GetByIdAsync(request.BorrowerId, ct);
            return b is null ? null : new BorrowerDetailsDto(b.UserId, b.Email, b.IsDisabled, b.DisabledReason, b.DisabledAtUtc);
        }
    }
}
