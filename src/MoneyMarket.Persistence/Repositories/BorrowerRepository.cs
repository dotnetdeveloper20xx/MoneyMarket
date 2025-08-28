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

        /// <summary>
        /// Returns the BorrowerProfile for the given user (Guid in AspNetUsers.Id).
        /// NOTE: Signature takes string (from ICurrentUserService); we parse once to Guid.
        /// </summary>
        public async Task<BorrowerProfile?> GetByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return null; // or throw new InvalidOperationException("Invalid user id");

            IQueryable<BorrowerProfile> query = _db.Set<BorrowerProfile>()
                .Include(p => p.Debts)
                .Include(p => p.Documents)
                .Include(p => p.AuditTrail);

            if (asNoTracking) query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync(p => p.UserId == userGuid, ct);
        }

        public Task AddAsync(BorrowerProfile profile, CancellationToken ct)
            => _db.AddAsync(profile, ct).AsTask();

        public void Update(BorrowerProfile profile)
        {
            var entry = _db.Entry(profile);
            if (entry.State == EntityState.Detached)
            {
                _db.Attach(profile);
                // EF change tracker will pick up modified owned collections if they are replaced
                // If you need to enforce full update: entry.State = EntityState.Modified;
            }
        }

        /// <summary>
        /// Checks existence by user id (string claim). Parses to Guid once, then Any on Guid column.
        /// </summary>
        public async Task<bool> ExistsForUserAsync(string userId, CancellationToken ct)
        {
            if (!Guid.TryParse(userId, out var userGuid))
                return false;

            return await _db.Set<BorrowerProfile>().AnyAsync(p => p.UserId == userGuid, ct);
        }

        public async Task<BorrowerProfile?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct)
        {
            IQueryable<BorrowerProfile> q = _db.Set<BorrowerProfile>()
                .Include(p => p.Debts)
                .Include(p => p.Documents)
                .Include(p => p.AuditTrail);

            if (asNoTracking) q = q.AsNoTracking();

            return await q.SingleOrDefaultAsync(p => p.Id == id, ct);
        }

        /// <summary>
        /// Returns paged Borrowers (not profiles) for CRM/views.
        /// </summary>
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

        /// <summary>
        /// Get a Borrower by its primary key (BorrowerId), not by user id.
        /// </summary>
        public Task<Borrower?> GetByIdAsync(Guid id, CancellationToken ct)
            => _db.Borrowers.FirstOrDefaultAsync(x => x.BorrowerId == id, ct);

        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
