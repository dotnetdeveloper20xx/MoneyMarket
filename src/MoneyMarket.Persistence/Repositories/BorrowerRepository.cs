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

            // add includes first (or as needed)
            query = query.Include(p => p.Debts);

            // apply tracking mode last, but still on IQueryable<T>
            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.SingleOrDefaultAsync(p => p.UserId == userId, ct);
        }

        //public async Task<BorrowerProfile?> GetByUserIdAsync(
        //                        string userId,
        //                        bool asNoTracking,
        //                        CancellationToken ct,
        //                        bool includeDebts = true)
        //{
        //    IQueryable<BorrowerProfile> query = _db.Set<BorrowerProfile>();
        //    if (includeDebts) query = query.Include(p => p.Debts);
        //    if (asNoTracking) query = query.AsNoTracking();
        //    return await query.SingleOrDefaultAsync(p => p.UserId == userId, ct);
        //}

        public async Task AddAsync(BorrowerProfile profile, CancellationToken ct)
            => await _db.AddAsync(profile, ct);

        public void Update(BorrowerProfile profile)
            => _db.Update(profile);

        public async Task<bool> ExistsForUserAsync(string userId, CancellationToken ct)
            => await _db.Set<BorrowerProfile>().AnyAsync(p => p.UserId == userId, ct);
    }
}
