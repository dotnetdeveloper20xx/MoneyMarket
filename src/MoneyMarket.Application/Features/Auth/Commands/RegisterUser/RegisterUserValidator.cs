using FluentValidation;

namespace MoneyMarket.Application.Features.Auth.Commands.RegisterUser
{
    public sealed class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
            RuleFor(x => x.Role).NotEmpty().Must(r => new[] { "Admin", "Borrower", "Lender" }.Contains(r));
        }
    }
}
