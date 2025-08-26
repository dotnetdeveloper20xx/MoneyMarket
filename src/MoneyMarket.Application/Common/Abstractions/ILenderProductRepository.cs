using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderProductRepository
    {
        Task<LenderProduct?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);
        Task AddAsync(LenderProduct product, CancellationToken ct);
        void Update(LenderProduct product);
        Task<(IReadOnlyList<LenderProduct> Items, int Total)> GetMinePagedAsync(Guid lenderId, int page, int size, CancellationToken ct);
        Task<(IReadOnlyList<LenderProduct> Items, int Total)> GetPublicPagedAsync(int page, int size, CancellationToken ct); // for borrowers
        Task SaveChangesAsync(CancellationToken ct);
    }
}
