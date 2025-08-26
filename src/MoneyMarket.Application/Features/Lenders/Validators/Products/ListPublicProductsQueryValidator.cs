using FluentValidation;
using MoneyMarket.Application.Features.Lenders.Queries.Products;

namespace MoneyMarket.Application.Features.Lenders.Validators.Products
{
    public sealed class ListPublicProductsQueryValidator : AbstractValidator<ListPublicProductsQuery>
    {
        public ListPublicProductsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
