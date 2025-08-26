using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{
    public sealed class EmploymentInfoDtoValidator : AbstractValidator<EmploymentInfoDto>
    {
        public EmploymentInfoDtoValidator()
        {
            RuleFor(x => x.EmployerName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.JobTitle).NotEmpty().MaximumLength(150);
            RuleFor(x => x.LengthOfEmployment).NotEmpty().MaximumLength(100);
            RuleFor(x => x.GrossAnnualIncome).GreaterThanOrEqualTo(0);
            RuleFor(x => x.AdditionalSources).MaximumLength(2000).When(x => x.AdditionalSources != null);
        }
    }
}
