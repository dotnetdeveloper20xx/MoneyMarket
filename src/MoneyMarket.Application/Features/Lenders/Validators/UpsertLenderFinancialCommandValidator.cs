using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Commands;

namespace MoneyMarket.Application.Features.Lenders.Validators
{
    public sealed class UpsertLenderFinancialCommandValidator : AbstractValidator<UpsertLenderFinancialCommand>
    {
        public UpsertLenderFinancialCommandValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage("Dto is required.")
                .SetValidator(new UpsertFinancialCapacityDtoValidator()!);
        }
    }
}
