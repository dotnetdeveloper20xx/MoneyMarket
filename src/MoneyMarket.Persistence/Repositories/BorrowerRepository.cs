using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Borrowers;
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
            => _db.Update(profile);

        public async Task<bool> ExistsForUserAsync(string userId, CancellationToken ct)
            => await _db.Set<BorrowerProfile>().AnyAsync(p => p.UserId == userId, ct);

        public async Task<BorrowerProfile?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct)
        {
            IQueryable<BorrowerProfile> q = _db.Set<BorrowerProfile>()
                .Include(p => p.Debts).Include(p => p.Documents).Include(p => p.AuditTrail);
            if (asNoTracking) q = q.AsNoTracking();
            return await q.SingleOrDefaultAsync(p => p.Id == id, ct);
        }
    }
}
