using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Persistence.Context;
using MoneyMarket.Persistence.Identity;
using MoneyMarket.Persistence.Repositories;

namespace MoneyMarket.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("DefaultConnection")
                 ?? throw new InvalidOperationException("Connection string 'DefaultConnection' missing");

        // DbContexts
        services.AddDbContext<IdentityDbContextMM>(o => o.UseSqlServer(conn));
        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(conn));

        // Map abstractions
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<AppDbContext>());

        // Repositories
        services.AddScoped<ILoanRepository, LoanRepository>();
        services.AddScoped<IFundingRepository, FundingRepository>();  
        services.AddScoped<ILenderProfileRepository, LenderProfileRepository>();

        // ASP.NET Identity (this registers UserManager / SignInManager / RoleManager)
        services
            .AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContextMM>()
            .AddSignInManager();

        services.AddScoped<IBorrowerRepository, BorrowerRepository>();
        services.AddScoped<ILenderRepository, LenderRepository>();

        services.AddScoped<ILenderApplicationRepository, LenderApplicationRepository>();
        services.AddScoped<ILenderProductRepository, LenderProductRepository>();


        return services;
    }
}

// Helper to centralize migration/seeding in Program.cs
public static class PersistenceMigrationRunner
{
    public static async Task RunAsync(IServiceProvider sp)
    {
        var idCtx = sp.GetRequiredService<IdentityDbContextMM>();
        await idCtx.Database.MigrateAsync();

        var appCtx = sp.GetRequiredService<AppDbContext>();
        await appCtx.Database.MigrateAsync();
    }
}
