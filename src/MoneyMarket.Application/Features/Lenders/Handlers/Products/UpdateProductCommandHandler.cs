using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands.Products;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;
using MoneyMarket.Application.Features.Lenders.Mappings;

namespace MoneyMarket.Application.Features.Lenders.Handlers.Products
{
    public sealed class UpdateProductCommandHandler
      : IRequestHandler<UpdateProductCommand, LenderProductViewDto>
    {
        private readonly ILenderProductRepository _products;
        private readonly ILenderRepository _lenders;
        private readonly ICurrentUserService _current;

        public UpdateProductCommandHandler(
            ILenderProductRepository products,
            ILenderRepository lenders,
            ICurrentUserService current)
            => (_products, _lenders, _current) = (products, lenders, current);

        public async Task<LenderProductViewDto> Handle(UpdateProductCommand request, CancellationToken ct)
        {
            // 1) Resolve current lender by user
            var lender = await _lenders.GetAggregateByUserIdAsync(_current.UserId, false, ct)
             ?? throw new InvalidOperationException("Lender aggregate not found.");

            if (lender.IsDisabled)
                throw new InvalidOperationException("Your account is on hold and cannot manage products.");

            var lenderId = lender.LenderId;

            // 2) Load product
            var product = await _products.GetByIdAsync(request.ProductId, asNoTracking: false, ct)
                          ?? throw new KeyNotFoundException("Product not found.");

            // 3) Ownership check
            if (product.LenderId != lender.LenderId)
                throw new UnauthorizedAccessException("You do not own this product.");

            // 4) Update with domain rules
            var d = request.Dto;
            product.UpdateTerms(d.MinAmount, d.MaxAmount, d.TermMonths, d.Instalments, d.InterestRate, _current.Email ?? "system");

            _products.Update(product);
            await _products.SaveChangesAsync(ct);

            return product.ToView();
        }
    }
}
