using MediatR;
using MoneyMarket.Application.Features.Fundings.DTOs;

namespace MoneyMarket.Application.Features.Fundings.Queries.MyFundings
{
    public sealed record MyFundingsQuery() : IRequest<IReadOnlyList<FundingSummaryDto>>;
}
