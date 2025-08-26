using MediatR;

namespace MoneyMarket.Application.Features.CRM.Commands
{
    public sealed record DisableLenderCommand(Guid LenderId, string? Reason) : IRequest<bool>;
}
