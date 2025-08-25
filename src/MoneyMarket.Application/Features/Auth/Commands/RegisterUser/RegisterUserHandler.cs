using MediatR;
using Microsoft.Extensions.Logging;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Common.Models;


namespace MoneyMarket.Application.Features.Auth.Commands.RegisterUser
{
    public sealed class RegisterUserHandler : IRequestHandler<RegisterUserCommand, ApiResponse<string>>
    {
        private readonly IIdentityService _identity;

        public RegisterUserHandler(IIdentityService identity) => _identity = identity;

        public async Task<ApiResponse<string>> Handle(RegisterUserCommand req, CancellationToken ct)
        {
            var (ok, userId, errors) = await _identity.RegisterAsync(req.Email, req.Password, req.Role);
            return ok
                ? ApiResponse<string>.SuccessResult(userId!, "User registered")
                : ApiResponse<string>.Fail(string.Join("; ", errors));
        }
    }
}





