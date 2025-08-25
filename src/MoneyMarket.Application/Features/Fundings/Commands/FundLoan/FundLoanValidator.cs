using FluentValidation;

namespace MoneyMarket.Application.Features.Fundings.Commands.FundLoan
{
    public sealed class FundLoanValidator : AbstractValidator<FundLoanCommand>
    {
        public FundLoanValidator()
        {
            RuleFor(x => x.LoanId).NotEmpty();
            RuleFor(x => x.LenderId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
