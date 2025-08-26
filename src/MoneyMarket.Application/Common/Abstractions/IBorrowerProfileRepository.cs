using MoneyMarket.Domain.Borrowers;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface IBorrowerProfileRepository
    {
        Task<(IReadOnlyList<BorrowerProfile> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<BorrowerProfile?> GetByIdAsync(Guid id, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
