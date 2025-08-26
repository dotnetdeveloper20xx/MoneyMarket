using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Lenders;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories
{
    public sealed class LenderProductRepository : ILenderProductRepository
    {
        private readonly AppDbContext _db;
        public LenderProductRepository(AppDbContext db) => _db = db;

        public Task<LenderProduct?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct)
            => (asNoTracking ? _db.LenderProducts.AsNoTracking() : _db.LenderProducts)
                .FirstOrDefaultAsync(x => x.LenderProductId == id, ct);

        public Task AddAsync(LenderProduct product, CancellationToken ct) => _db.LenderProducts.AddAsync(product, ct).AsTask();
        public void Update(LenderProduct product) => _db.LenderProducts.Update(product);

        public async Task<(IReadOnlyList<LenderProduct> Items, int Total)> GetMinePagedAsync(Guid lenderId, int page, int size, CancellationToken ct)
        {
            var q = _db.LenderProducts.AsNoTracking().Where(x => x.LenderId == lenderId && x.IsActive);
            var total = await q.CountAsync(ct);
            var items = await q.OrderByDescending(x => x.LastModifiedAtUtc ?? x.CreatedAtUtc)
                               .Skip((page - 1) * size).Take(size).ToListAsync(ct);
            return (items, total);
        }

        public async Task<(IReadOnlyList<LenderProduct> Items, int Total)> GetPublicPagedAsync(int page, int size, CancellationToken ct)
        {
            var q = _db.LenderProducts.AsNoTracking().Where(x => x.IsActive);
            var total = await q.CountAsync(ct);
            var items = await q.OrderByDescending(x => x.LastModifiedAtUtc ?? x.CreatedAtUtc)
                               .Skip((page - 1) * size).Take(size).ToListAsync(ct);
            return (items, total);
        }

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
