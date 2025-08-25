using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Loans.Dtos;
using MoneyMarket.Application.Features.Loans.Queries.GetOpenLoans;

public sealed class GetOpenLoansHandler : IRequestHandler<GetOpenLoansQuery, IReadOnlyList<LoanSummaryDto>>
{
    private readonly ILoanRepository _loans;
    public GetOpenLoansHandler(ILoanRepository loans) => _loans = loans;

    public Task<IReadOnlyList<LoanSummaryDto>> Handle(GetOpenLoansQuery request, CancellationToken ct)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var size = request.Size <= 0 ? 20 : request.Size;

        // Repository returns projected summaries (no DbContext in handler)
        return _loans.GetOpenSummariesAsync(page, size, ct);
    }
}
