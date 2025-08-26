using MediatR;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Application.Features.Borrowers.Queries
{
    public sealed record GetMyProfileQuery() : IRequest<ApiResponse<BorrowerProfileViewDto>>;
}
