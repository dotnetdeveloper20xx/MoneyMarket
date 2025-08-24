using Microsoft.EntityFrameworkCore;

namespace MoneyMarket.Persistence.Context;

public class AppDbContext : DbContext
{
    //public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    //public DbSet<Borrower> Borrowers { get; set; }
    //public DbSet<Lender> Lenders { get; set; }
    //public DbSet<Loan> Loans { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    base.OnModelCreating(modelBuilder);
    //    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    //}
}
