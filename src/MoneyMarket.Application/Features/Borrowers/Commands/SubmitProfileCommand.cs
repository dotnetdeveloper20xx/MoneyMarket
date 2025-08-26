using MediatR;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Borrowers.Commands
{
    public sealed record SubmitProfileCommand() : IRequest<ApiResponse<bool>>;
}
