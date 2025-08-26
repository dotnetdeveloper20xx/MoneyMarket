using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Application.Features.Borrowers.Commands
{
    public sealed record UpsertFinancialInfoCommand(EmploymentInfoDto Data)
      : IRequest<ApiResponse<bool>>, ITransactionalRequest;
}
