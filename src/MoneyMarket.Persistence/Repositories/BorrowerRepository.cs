using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Borrowers;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories
{
    public sealed class BorrowerRepository : IBorrowerRepository
    {
        private readonly AppDbContext _db;
        public BorrowerRepository(AppDbContext db) => _db = db;

        public async Task<BorrowerProfile?> GetByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct)
        {
            IQueryable<BorrowerProfile> query = _db.Set<BorrowerProfile>();
            query = query.Include(p => p.Debts)
                         .Include(p => p.Documents)
                         .Include(p => p.AuditTrail);
            if (asNoTracking) query = query.AsNoTracking();
            return await query.SingleOrDefaultAsync(p => p.UserId == userId, ct);
        }

        public async Task AddAsync(BorrowerProfile profile, CancellationToken ct)
            => await _db.AddAsync(profile, ct);

        public void Update(BorrowerProfile profile)
        {
            var entry = _db.Entry(profile);
            if (entry.State == EntityState.Detached)
            {
                _db.Attach(profile);
            }
        }

        public async Task<bool> ExistsForUserAsync(string userId, CancellationToken ct)
            => await _db.Set<BorrowerProfile>().AnyAsync(p => p.UserId == userId, ct);

        public async Task<BorrowerProfile?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct)
        {
            IQueryable<BorrowerProfile> q = _db.Set<BorrowerProfile>()
                .Include(p => p.Debts).Include(p => p.Documents).Include(p => p.AuditTrail);
            if (asNoTracking) q = q.AsNoTracking();
            return await q.SingleOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<(IReadOnlyList<Borrower> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct)
        {
            var query = _db.Borrowers.AsNoTracking();
            var total = await query.CountAsync(ct);
            var items = await query
                .OrderBy(x => x.Email)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(ct);
            return (items, total);
        }

        public Task<Borrower?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Borrowers.FirstOrDefaultAsync(x => x.UserId == id, ct);

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
