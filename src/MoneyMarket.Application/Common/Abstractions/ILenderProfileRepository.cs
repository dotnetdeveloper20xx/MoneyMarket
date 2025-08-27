using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderProfileRepository
    {
        Task<(IReadOnlyList<Lender> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<Lender?> GetByIdAsync(Guid id, CancellationToken ct);
        
        Task<Lender?> GetByUserIdAsync(Guid userId, bool asNoTracking, CancellationToken ct);
        Task AddAsync(Lender lender, CancellationToken ct);

        // Keep if used elsewhere; DO NOT call from handlers that use UoW.
        Task SaveChangesAsync(CancellationToken ct);
    }
}
