using Microsoft.AspNetCore.Identity;
using MoneyMarket.Application.Common.Abstractions;
using MoneyMarket.Persistence.Identity;

namespace MoneyMarket.Infrastructure.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _users;
        private readonly SignInManager<ApplicationUser> _signIn;
        private readonly RoleManager<ApplicationRole> _roles;
        private readonly IJwtTokenService _jwt;

        public IdentityService(
            UserManager<ApplicationUser> users,
            SignInManager<ApplicationUser> signIn,
            RoleManager<ApplicationRole> roles,
            IJwtTokenService jwt)
        { _users = users; _signIn = signIn; _roles = roles; _jwt = jwt; }

        public async Task<(bool Succeeded, string? UserId, string[] Errors)> RegisterAsync(string email, string password, string role)
        {
            var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var create = await _users.CreateAsync(user, password);
            if (!create.Succeeded) return (false, null, create.Errors.Select(e => e.Description).ToArray());

            if (!await _roles.RoleExistsAsync(role)) await _roles.CreateAsync(new ApplicationRole { Name = role });
            await _users.AddToRoleAsync(user, role);

            return (true, user.Id.ToString(), Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string Token, string[] Errors)> LoginAsync(string email, string password)
        {
            var user = await _users.FindByEmailAsync(email);
            if (user is null) return (false, "", new[] { "User not found" });

            var ok = await _signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!ok.Succeeded) return (false, "", new[] { "Invalid credentials" });

            var roles = await _users.GetRolesAsync(user);
            var token = _jwt.CreateToken(user.Id.ToString(), email, roles);
            return (true, token, Array.Empty<string>());
        }
    }
}
