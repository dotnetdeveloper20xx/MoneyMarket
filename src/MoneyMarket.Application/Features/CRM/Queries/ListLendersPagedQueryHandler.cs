using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.CRM.Dtos;

namespace MoneyMarket.Application.Features.CRM.Queries
{
    public sealed class ListLendersPagedQueryHandler : IRequestHandler<ListLendersPagedQuery, PagedResult<LenderRowDto>>
    {
        private readonly ILenderProfileRepository _repo;
        public ListLendersPagedQueryHandler(ILenderProfileRepository repo) => _repo = repo;


        public async Task<PagedResult<LenderRowDto>> Handle(ListLendersPagedQuery request, CancellationToken ct)
        {
            var (items, total) = await _repo.GetPagedAsync(request.PageNumber, request.PageSize, ct);
            var rows = items.Select(l => new LenderRowDto(l.Id, l.Email, l.DisplayName, l.IsDisabled)).ToList();
            return PagedResult<LenderRowDto>.Create(rows, request.PageNumber, request.PageSize, total);
        }
    }
}
