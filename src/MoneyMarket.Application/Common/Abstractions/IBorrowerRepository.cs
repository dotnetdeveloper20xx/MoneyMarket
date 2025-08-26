using MoneyMarket.Domain.Borrowers;
using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface IBorrowerRepository
    {
        Task<BorrowerProfile?> GetByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct);
        Task AddAsync(BorrowerProfile profile, CancellationToken ct);
        void Update(BorrowerProfile profile);

        // Convenience helpers (optional but handy)
        Task<bool> ExistsForUserAsync(string userId, CancellationToken ct);
        Task<BorrowerProfile?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);
        Task<(IReadOnlyList<Borrower> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<Borrower?> GetByIdAsync(Guid id, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);

    }
}
