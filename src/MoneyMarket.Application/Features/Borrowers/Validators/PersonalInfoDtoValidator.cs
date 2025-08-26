using FluentValidation;
using MoneyMarket.Application.Features.Borrowers.Dtos;

namespace MoneyMarket.Application.Features.Borrowers.Validators
{
    public sealed class PersonalInfoDtoValidator : AbstractValidator<PersonalInfoDto>
    {
        public PersonalInfoDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(150);
            RuleFor(x => x.DateOfBirth)
                .LessThan(DateTime.UtcNow.Date)
                .Must(d =>
                {
                    var age = DateTime.UtcNow.Year - d.Year;
                    if (d.Date > DateTime.UtcNow.Date.AddYears(-age)) age--;
                    return age >= 18;
                }).WithMessage("Borrower must be 18 or older.");
            RuleFor(x => x.NationalIdNumber).NotEmpty().MaximumLength(150);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Address).NotNull();
            RuleFor(x => x.Address.House).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Address.Street).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Address.City).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Address.Country).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Address.PostCode).NotEmpty().MaximumLength(50);
        }
    }
}
