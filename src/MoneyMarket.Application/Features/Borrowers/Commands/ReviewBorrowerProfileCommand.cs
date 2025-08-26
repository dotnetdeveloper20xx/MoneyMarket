using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Borrowers.Commands
{
    public sealed record ReviewBorrowerProfileCommand(Guid BorrowerProfileId, bool Approve, string? Reason)
     : IRequest<ApiResponse<bool>>, ITransactionalRequest;
}
