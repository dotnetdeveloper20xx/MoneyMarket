using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Commands;

namespace MoneyMarket.Application.Features.Lenders.Validators
{
    public sealed class UpsertLenderRiskCommandValidator : AbstractValidator<UpsertLenderRiskCommand>
    {
        public UpsertLenderRiskCommandValidator()
        {
            RuleFor(x => x.Dto)
                .NotNull().WithMessage("Dto is required.")                
                .SetValidator(new UpsertRiskManagementDtoValidator()!);

        }
    }
}
