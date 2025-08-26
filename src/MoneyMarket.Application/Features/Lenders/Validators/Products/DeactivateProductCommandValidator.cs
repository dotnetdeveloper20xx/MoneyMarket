using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Commands.Products;

namespace MoneyMarket.Application.Features.Lenders.Validators.Products
{
    public sealed class DeactivateProductCommandValidator : AbstractValidator<DeactivateProductCommand>
    {
        public DeactivateProductCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
        }
    }
}
