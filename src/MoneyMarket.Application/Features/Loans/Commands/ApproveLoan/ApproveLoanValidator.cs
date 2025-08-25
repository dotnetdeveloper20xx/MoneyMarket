using FluentValidation;

namespace MoneyMarket.Application.Features.Loans.Commands.ApproveLoan
{
    public sealed class ApproveLoanValidator : AbstractValidator<ApproveLoanCommand>
    {
        public ApproveLoanValidator()
        {
            RuleFor(x => x.LoanId).NotEmpty();
            RuleFor(x => x.ApprovedAmount).GreaterThan(0);
            RuleFor(x => x.InterestRate).InclusiveBetween(0, 100);
            RuleFor(x => x.Fees).GreaterThanOrEqualTo(0);
        }
    }

}
