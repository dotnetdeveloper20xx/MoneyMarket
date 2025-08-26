using MediatR;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.CRM.Dtos;

namespace MoneyMarket.Application.Features.CRM.Queries
{
    public sealed record ListLendersPagedQuery(int PageNumber = 1, int PageSize = 20)
 : IRequest<PagedResult<LenderRowDto>>;
}
