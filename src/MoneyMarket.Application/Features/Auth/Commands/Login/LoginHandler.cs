using MediatR;
using Microsoft.AspNetCore.Identity;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;
using MoneyMarket.Application.Features.Auth.Dtos;

namespace MoneyMarket.Application.Features.Auth.Commands.Login
{

    public sealed class LoginHandler : IRequestHandler<LoginCommand, ApiResponse<AuthResult>>
    {
        private readonly IIdentityService _identity;

        public LoginHandler(IIdentityService identity) => _identity = identity;

        public async Task<ApiResponse<AuthResult>> Handle(LoginCommand req, CancellationToken ct)
        {
            var (ok, token, errors) = await _identity.LoginAsync(req.Email, req.Password);
            if (!ok) return ApiResponse<AuthResult>.Fail(string.Join("; ", errors), "invalid_login");
            return ApiResponse<AuthResult>.SuccessResult(new AuthResult(token, "Bearer", DateTime.UtcNow.AddHours(1)), "Logged in");
        }
    }
}
