using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{
    public sealed class UpsertFinancialInfoCommandValidator : AbstractValidator<UpsertFinancialInfoCommand>
    {
        public UpsertFinancialInfoCommandValidator(EmploymentInfoDtoValidator dtoValidator)
        {
            RuleFor(x => x.Data).SetValidator(dtoValidator);
        }
    }
}
