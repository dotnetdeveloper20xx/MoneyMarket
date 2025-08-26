using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{
    public sealed class DebtItemDtoValidator : AbstractValidator<DebtItemDto>
    {
        public DebtItemDtoValidator()
        {
            RuleFor(x => x.LenderName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.DebtType).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0);
        }
    }
}
