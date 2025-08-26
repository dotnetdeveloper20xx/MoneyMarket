using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands.Products;

namespace MoneyMarket.Application.Features.Lenders.Handlers.Products
{
    public sealed class DeactivateProductCommandHandler
       : IRequestHandler<DeactivateProductCommand, bool>
    {
        private readonly ILenderProductRepository _products;
        private readonly ILenderRepository _lenders;
        private readonly ICurrentUserService _current;

        public DeactivateProductCommandHandler(
            ILenderProductRepository products,
            ILenderRepository lenders,
            ICurrentUserService current)
            => (_products, _lenders, _current) = (products, lenders, current);

        public async Task<bool> Handle(DeactivateProductCommand request, CancellationToken ct)
        {
            var lender = await _lenders.GetByUserIdAsync(_current.UserId, asNoTracking: false, ct)
                         ?? throw new InvalidOperationException("Lender profile not found.");

            if (lender.IsDisabled)
                throw new InvalidOperationException("Your account is on hold and cannot manage products.");

            var product = await _products.GetByIdAsync(request.ProductId, asNoTracking: false, ct)
                          ?? throw new KeyNotFoundException("Product not found.");

            if (product.LenderId != lender.Id)
                throw new UnauthorizedAccessException("You do not own this product.");

            if (!product.IsActive) return true; // already inactive
            product.Deactivate(_current.Email ?? "system");

            _products.Update(product);
            await _products.SaveChangesAsync(ct);
            return true;
        }
    }
}
