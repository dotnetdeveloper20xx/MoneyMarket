using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MoneyMarket.Persistence.Context;

public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContextMM>
{
    public IdentityDbContextMM CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContextMM>();
        // ⚠️ use same name as your appsettings.json connection string
        optionsBuilder.UseSqlServer("Server=.;Database=MoneyMarketDb;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True");

        return new IdentityDbContextMM(optionsBuilder.Options);
    }
}
