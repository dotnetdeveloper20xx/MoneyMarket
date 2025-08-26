using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Application.Common.Abstractions
{
    public interface ILenderApplicationRepository
    {
        Task<LenderApplication?> GetMineAsync(Guid userId, bool asNoTracking, CancellationToken ct);
        Task<LenderApplication?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);
        Task AddAsync(LenderApplication app, CancellationToken ct);
        void Update(LenderApplication app);
        Task SaveChangesAsync(CancellationToken ct);
    }
}
