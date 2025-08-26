using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Validators
{
    public sealed class UpsertFinancialCapacityDtoValidator : AbstractValidator<UpsertFinancialCapacityDto>
    {
        public UpsertFinancialCapacityDtoValidator()
        {
            RuleFor(x => x.FundingSourceType)
                .NotEmpty().WithMessage("FundingSourceType is required.");

            RuleFor(x => x.FundingSourceDescription)
                .NotEmpty().WithMessage("FundingSourceDescription is required.")
                .MaximumLength(2000);

            RuleFor(x => x.CapitalReserveDocuments)
                .NotNull().WithMessage("CapitalReserveDocuments must be provided (can be empty).");
            RuleForEach(x => x.CapitalReserveDocuments!)
                .NotEmpty().WithMessage("Each document reference must be non-empty.");
        }
    }
}
