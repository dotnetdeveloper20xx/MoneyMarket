using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderProfileRepository
    {
        Task<(IReadOnlyList<LenderProfile> Items, int Total)> GetPagedAsync(int page, int size, CancellationToken ct);
        Task<LenderProfile?> GetByIdAsync(Guid id, CancellationToken ct);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
