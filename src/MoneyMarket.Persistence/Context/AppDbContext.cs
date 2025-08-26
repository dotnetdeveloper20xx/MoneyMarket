using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Borrowers;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;

namespace MoneyMarket.Persistence.Context;

public class AppDbContext : DbContext, IAppDbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<Lender> Lenders => Set<Lender>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Funding> Fundings => Set<Funding>();
    public DbSet<RepaymentInstallment> RepaymentInstallments => Set<RepaymentInstallment>();
    public DbSet<LenderApplication> LenderApplications => Set<LenderApplication>();
    public DbSet<LenderProduct> LenderProducts => Set<LenderProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    // Domain event dispatching/outbox could be added here later.
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);
}
