using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderRepository
    {
        //Task<LenderProfile?> GetByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct);
        //Task AddAsync(LenderProfile lender, CancellationToken ct);
        //void Update(LenderProfile lender);

        // Convenience helpers
        Task<bool> ExistsForUserAsync(string userId, CancellationToken ct);
        Task<Lender?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

        // For CRM dashboard (list all lenders, not just profile-level)
        Task<(IReadOnlyList<Lender> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<Lender?> GetByIdAsync(Guid id, CancellationToken ct);

        // NEW: fetch aggregate by user id (to get LenderId)
        Task<Lender?> GetAggregateByUserIdAsync(string userId, bool asNoTracking, CancellationToken ct);

        Task SaveChangesAsync(CancellationToken ct);
    }
}
