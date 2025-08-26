using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Commands.Products;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Validators.Products
{
    public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();

            RuleFor(x => x.Dto).NotNull();
            RuleFor(x => x.Dto.Name).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Dto.MinAmount).GreaterThan(0);
            RuleFor(x => x.Dto.MaxAmount)
                .GreaterThan(0)
                .GreaterThanOrEqualTo(x => x.Dto.MinAmount);
            RuleFor(x => x.Dto.TermMonths).GreaterThan(0);
            RuleFor(x => x.Dto.Instalments).GreaterThan(0);
            RuleFor(x => x.Dto.InterestRate)
                .GreaterThanOrEqualTo(LenderProduct.MinInterestRate)
                .WithMessage($"Interest rate must be ≥ {LenderProduct.MinInterestRate:P0}");
        }
    }
}
