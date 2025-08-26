using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.CRM.Dtos;

namespace MoneyMarket.Application.Features.CRM.Queries
{
    public sealed class ListBorrowersPagedQueryHandler : IRequestHandler<ListBorrowersPagedQuery, PagedResult<BorrowerRowDto>>
    {
        private readonly IBorrowerRepository _repo;
        public ListBorrowersPagedQueryHandler(IBorrowerRepository repo) => _repo = repo;

        public async Task<PagedResult<BorrowerRowDto>> Handle(ListBorrowersPagedQuery request, CancellationToken ct)
        {
            var (items, total) = await _repo.GetPagedAsync(request.PageNumber, request.PageSize, ct);
            var rows = items.Select(b => new BorrowerRowDto(b.UserId, b.Email, b.IsDisabled)).ToList();
            return PagedResult<BorrowerRowDto>.Create(rows, request.PageNumber, request.PageSize, total);
        }
    }
}
