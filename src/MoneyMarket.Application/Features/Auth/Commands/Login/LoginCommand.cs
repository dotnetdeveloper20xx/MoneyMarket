using MediatR;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Auth.Dtos;

namespace MoneyMarket.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(string Email, string Password) : IRequest<ApiResponse<AuthResult>>;
}
