using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Commands;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{
    public sealed class UpsertDebtsCommandValidator : AbstractValidator<UpsertDebtsCommand>
    {
        public UpsertDebtsCommandValidator(DebtItemDtoValidator itemValidator)
        {
            RuleFor(x => x.Debts).NotNull();
            RuleForEach(x => x.Debts).SetValidator(itemValidator);
        }
    }
}
