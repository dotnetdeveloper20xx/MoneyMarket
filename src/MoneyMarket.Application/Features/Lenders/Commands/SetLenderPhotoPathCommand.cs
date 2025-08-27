using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Commands
{
    /// <summary>Sets (or clears) the current user's lender profile photo path.</summary>
    public sealed record SetLenderPhotoPathCommand(string? PhotoPath)
        : IRequest<LenderProfileDto>, ITransactionalRequest;
}
