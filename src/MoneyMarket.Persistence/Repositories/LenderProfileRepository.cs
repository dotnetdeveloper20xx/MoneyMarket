using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories
{
    public sealed class LenderProfileRepository : ILenderProfileRepository
    {
        private readonly AppDbContext _ctx;
        public LenderProfileRepository(AppDbContext ctx) => _ctx = ctx;

        public async Task<(IReadOnlyList<Lender> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct)
        {
            var q = _ctx.Set<Lender>().AsNoTracking();
            var total = await q.CountAsync(ct);
            var items = await q.OrderBy(x => x.BusinessName)
                               .Skip((page - 1) * size)
                               .Take(size)
                               .ToListAsync(ct);
            return (items, total);
        }

        public Task<Lender?> GetByIdAsync(Guid id, CancellationToken ct)
            => _ctx.Set<Lender>().AsNoTracking().FirstOrDefaultAsync(x => x.LenderId == id, ct);

        public async Task<Lender?> GetByUserIdAsync(Guid userId, bool asNoTracking, CancellationToken ct)
        {
            IQueryable<Lender> q = _ctx.Set<Lender>();
            if (asNoTracking) q = q.AsNoTracking();
            return await q.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        public Task AddAsync(Lender lender, CancellationToken ct)
            => _ctx.Set<Lender>().AddAsync(lender, ct).AsTask();

        // Keep for non-UoW call sites
        public Task SaveChangesAsync(CancellationToken ct)
            => _ctx.SaveChangesAsync(ct);
    }
}
