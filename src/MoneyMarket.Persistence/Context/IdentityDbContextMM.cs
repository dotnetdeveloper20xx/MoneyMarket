using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Persistence.Context
{
    public class IdentityDbContextMM
     : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public IdentityDbContextMM(DbContextOptions<IdentityDbContextMM> options) : base(options) { }
    }
}
