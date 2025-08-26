using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories
{
    public sealed class LenderRepository : ILenderRepository
    {
        private readonly AppDbContext _db;
        public LenderRepository(AppDbContext db) => _db = db;

        public async Task<(IReadOnlyList<Lender> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct)
        {
            var query = _db.Lenders.AsNoTracking();
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(x => x.Email)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);
            return (items, total);
        }

        public Task<Lender?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Lenders.FirstOrDefaultAsync(x => x.UserId == id, ct);

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
