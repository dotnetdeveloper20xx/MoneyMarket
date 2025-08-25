using MediatR;
using MoneyMarket.Application.Common.Models;

namespace MoneyMarket.Application.Features.Auth.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(string Email, string Password, string Role) : IRequest<ApiResponse<string>>;
}


