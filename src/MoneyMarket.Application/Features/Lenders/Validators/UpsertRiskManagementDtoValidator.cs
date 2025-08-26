using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Dtos;

namespace MoneyMarket.Application.Features.Lenders.Validators
{
    public sealed class UpsertRiskManagementDtoValidator : AbstractValidator<UpsertRiskManagementDto>
    {
        public UpsertRiskManagementDtoValidator()
        {
            RuleFor(x => x.UnderwritingPolicy)
                .NotEmpty().WithMessage("UnderwritingPolicy is required.");

            RuleFor(x => x.RiskAssessmentTools)
                .NotNull().WithMessage("RiskAssessmentTools must be provided (can be empty).");
            RuleForEach(x => x.RiskAssessmentTools!)
                .NotEmpty().WithMessage("Each tool entry must be non-empty.");

            RuleFor(x => x.PaymentCollectionProcess)
                .NotEmpty().WithMessage("PaymentCollectionProcess is required.");

            RuleFor(x => x.CommunicationPlan)
                .NotEmpty().WithMessage("CommunicationPlan is required.");

            RuleFor(x => x.DefaultHandlingStrategy)
                .NotEmpty().WithMessage("DefaultHandlingStrategy is required.");

            RuleFor(x => x.PricingStrategy)
                .NotEmpty().WithMessage("PricingStrategy is required.");
        }
    }
}
