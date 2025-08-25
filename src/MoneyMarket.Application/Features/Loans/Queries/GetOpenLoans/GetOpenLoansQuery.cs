using MediatR;
using MoneyMarket.Application.Features.Loans.Dtos;

namespace MoneyMarket.Application.Features.Loans.Queries.GetOpenLoans
{
    public sealed record GetOpenLoansQuery(int Page = 1, int Size = 20) : IRequest<IReadOnlyList<LoanSummaryDto>>;
}
