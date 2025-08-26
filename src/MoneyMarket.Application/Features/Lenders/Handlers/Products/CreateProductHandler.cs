using MediatR;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Application.Features.Lenders.Commands.Products;
using MoneyMarket.Application.Features.Lenders.Dtos.Products;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Features.Lenders.Handlers.Products
{
    public sealed class CreateProductHandler : IRequestHandler<CreateProductCommand, LenderProductViewDto>
    {
        private readonly ILenderRepository _lenderRepo;       // to fetch current lenderId by userId (you already have this)
        private readonly ILenderProductRepository _repo;
        private readonly ICurrentUserService _current;

        public CreateProductHandler(ILenderRepository lenderRepo, ILenderProductRepository repo, ICurrentUserService current)
            => (_lenderRepo, _repo, _current) = (lenderRepo, repo, current);

        public async Task<LenderProductViewDto> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var userId = _current.UserId;
            var lender = await _lenderRepo.GetAggregateByUserIdAsync(_current.UserId, false, ct)
             ?? throw new InvalidOperationException("Lender aggregate not found.");

            if (lender.IsDisabled)
                throw new InvalidOperationException("Your account is on hold and cannot manage products.");

            var lenderId = lender.LenderId;

            var dto = request.Dto;
            var prod = LenderProduct.Create(
                lender.LenderId, dto.Name, dto.TermType,
                dto.MinAmount, dto.MaxAmount, dto.TermMonths, dto.Instalments,
                dto.InterestRate, _current.Email ?? "system");

            await _repo.AddAsync(prod, ct);
            await _repo.SaveChangesAsync(ct);

            return new LenderProductViewDto(
                prod.LenderProductId, prod.Name, prod.TermType, prod.MinAmount, prod.MaxAmount,
                prod.TermMonths, prod.Instalments, prod.InterestRate,
                LenderProduct.PlatformShare, prod.LenderMargin(), prod.IsActive);
        }
    }
}
