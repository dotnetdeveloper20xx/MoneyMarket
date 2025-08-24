namespace MoneyMarket.Persistence.Seed;

public static class AppDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        //using var scope = services.CreateScope();
        //var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        //await context.Database.MigrateAsync();

        //if (!await context.Borrowers.AnyAsync())
        //{
        //    context.Borrowers.Add(new Borrower { FirstName = "John", LastName = "Doe", Email = "john@demo.com" });
        //}

        //if (!await context.Lenders.AnyAsync())
        //{
        //    context.Lenders.Add(new Lender { BusinessName = "Acme Lending Ltd", ComplianceStatement = "Compliant" });
        //}

        //await context.SaveChangesAsync();
    }
}
