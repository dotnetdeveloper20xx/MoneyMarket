using FluentValidation;

namespace MoneyMarket.Application.Features.Loans.Commands.SubmitLoan
{
    public sealed class SubmitLoanValidator : AbstractValidator<SubmitLoanCommand>
    {
        public SubmitLoanValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.TargetAmount).GreaterThan(0);
            RuleFor(x => x.Apr).InclusiveBetween(0.01m, 100m);
            RuleFor(x => x.BorrowerId).NotEmpty();
        }
    }
}
