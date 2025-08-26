using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderProfileRepository
    {
        Task<(IReadOnlyList<Lender> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<Lender?> GetByIdAsync(Guid id, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
