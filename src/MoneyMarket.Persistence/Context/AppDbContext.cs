using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Domain.Borrowers;
using MoneyMarket.Domain.Entities;
using MoneyMarket.Domain.Lenders;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Persistence.Context;

public sealed class AppDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IAppDbContext, IUnitOfWork
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Domain tables
    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<BorrowerProfile> BorrowerProfiles => Set<BorrowerProfile>();
    public DbSet<Lender> Lenders => Set<Lender>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Funding> Fundings => Set<Funding>();
    public DbSet<RepaymentInstallment> RepaymentInstallments => Set<RepaymentInstallment>();
    public DbSet<LenderApplication> LenderApplications => Set<LenderApplication>();
    public DbSet<LenderProduct> LenderProducts => Set<LenderProduct>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Optional: explicitly keep Identity default table names (AspNetUsers, …)
        modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
        modelBuilder.Entity<ApplicationRole>().ToTable("AspNetRoles");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AspNetUserRoles");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AspNetUserClaims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AspNetUserLogins");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AspNetRoleClaims");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AspNetUserTokens");

        // Apply all IEntityTypeConfiguration<T> from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        => base.SaveChangesAsync(ct);
}
