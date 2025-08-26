using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{
    public sealed class UpsertPersonalInfoCommandValidator : AbstractValidator<UpsertPersonalInfoCommand>
    {
        public UpsertPersonalInfoCommandValidator(PersonalInfoDtoValidator dtoValidator)
        {
            RuleFor(x => x.Data).SetValidator(dtoValidator);
        }
    }
}
