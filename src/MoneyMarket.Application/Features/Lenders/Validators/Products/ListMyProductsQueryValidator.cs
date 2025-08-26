using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Queries.Products;

namespace MoneyMarket.Application.Features.Lenders.Validators.Products
{
    public sealed class ListMyProductsQueryValidator : AbstractValidator<ListMyProductsQuery>
    {
        public ListMyProductsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
