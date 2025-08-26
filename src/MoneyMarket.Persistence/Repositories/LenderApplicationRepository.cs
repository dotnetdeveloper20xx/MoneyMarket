using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Lenders;
using MoneyMarket.Persistence.Context;

namespace MoneyMarket.Persistence.Repositories
{
    public sealed class LenderApplicationRepository : ILenderApplicationRepository
    {
        private readonly AppDbContext _db;
        public LenderApplicationRepository(AppDbContext db) => _db = db;

        public Task<LenderApplication?> GetMineAsync(Guid userId, bool asNoTracking, CancellationToken ct)
            => (asNoTracking ? _db.LenderApplications.AsNoTracking() : _db.LenderApplications)
                .FirstOrDefaultAsync(x => x.UserId == userId, ct);

        public Task<LenderApplication?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct)
            => (asNoTracking ? _db.LenderApplications.AsNoTracking() : _db.LenderApplications)
                .FirstOrDefaultAsync(x => x.LenderApplicationId == id, ct);

        public Task AddAsync(LenderApplication app, CancellationToken ct) => _db.LenderApplications.AddAsync(app, ct).AsTask();
        public void Update(LenderApplication app) => _db.LenderApplications.Update(app);
        public Task SaveChangesAsync(CancellationToken ct) => _db.SaveChangesAsync(ct);
    }
}
