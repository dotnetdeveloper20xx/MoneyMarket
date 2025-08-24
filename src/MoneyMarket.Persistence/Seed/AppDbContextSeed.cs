using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MoneyMarket.Persistence.Identity;

public static class IdentitySeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleMgr = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        var roles = new[] { "Admin", "Borrower", "Lender", "CRM", "Support" };
        foreach (var role in roles)
            if (!await roleMgr.RoleExistsAsync(role))
                await roleMgr.CreateAsync(new ApplicationRole { Name = role });

        var email = "fazi@moneymarket.com";
        var user = await userMgr.FindByEmailAsync(email);
        if (user is null)
        {
            user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var res = await userMgr.CreateAsync(user, "Pa$$0rd123");
            if (!res.Succeeded)
            {
                var msg = string.Join("; ", res.Errors.Select(e => e.Description));
                throw new Exception($"Admin user creation failed: {msg}");
            }
        }
        if (!await userMgr.IsInRoleAsync(user, "Admin"))
            await userMgr.AddToRoleAsync(user, "Admin");
    }
}
