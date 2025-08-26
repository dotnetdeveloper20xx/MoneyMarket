using MediatR;

namespace MoneyMarket.Application.Features.CRM.Commands
{
    public sealed record DisableBorrowerCommand(Guid BorrowerId, string? Reason) : IRequest<bool>;
}
