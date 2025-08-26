using MoneyMarket.Domain.Entities;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderRepository
    {
        Task<(IReadOnlyList<Lender> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<Lender?> GetByIdAsync(Guid id, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
