using Microsoft.AspNetCore.Identity;

namespace MoneyMarket.Persistence.Identity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        // add audit fields if needed
    }
}
