using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories
{
    public sealed class LenderRepository : ILenderRepository
    {
        private readonly AppDbContext _db;
        public LenderRepository(AppDbContext db) => _db = db;

        public async Task<LenderProfile?> GetByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct)
        {
            var query = _db.Set<LenderProfile>().AsQueryable();
            if (asNoTracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.UserId == userId, ct);
        }

        public async Task AddAsync(LenderProfile lender, CancellationToken ct)
        {
            await _db.Set<LenderProfile>().AddAsync(lender, ct);
        }

        public void Update(LenderProfile lender)
        {
            _db.Set<LenderProfile>().Update(lender);
        }

        public async Task<bool> ExistsForUserAsync(string userId, CancellationToken ct)
        {
            return await _db.Set<LenderProfile>().AnyAsync(x => x.UserId == userId, ct);
        }

        public async Task<LenderProfile?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct)
        {
            var query = _db.Set<LenderProfile>().AsQueryable();
            if (asNoTracking) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

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

        public Task SaveChangesAsync(CancellationToken ct)
            => _db.SaveChangesAsync(ct);

        public async Task<Lender?> GetAggregateByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct)
        {
            var q = _db.Lenders.AsQueryable();
            if (asNoTracking) q = q.AsNoTracking();

            // Assuming Lender.UserId is a string in your identity system.
            // If it’s Guid, adapt the parameter/parse accordingly.

            if (!Guid.TryParse(userId, out var userGuid))
                throw new InvalidOperationException("Invalid user id claim");
            return await q.FirstOrDefaultAsync(x => x.UserId.ToString() == userId || x.UserId == userGuid, ct);
        }
    }
}
