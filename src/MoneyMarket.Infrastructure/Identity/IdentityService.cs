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

        public async Task AddUserToRoleAsync(Guid userId, string role, CancellationToken ct)
        {
            // Note: ASP.NET Identity APIs don’t take CancellationToken; we ignore 'ct' here
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new InvalidOperationException("User not found.");

            if (!await _roles.RoleExistsAsync(role))
            {
                var createRole = await _roles.CreateAsync(new ApplicationRole { Name = role });
                if (!createRole.Succeeded)
                    throw new InvalidOperationException("Failed to create role: " +
                        string.Join(", ", createRole.Errors.Select(e => e.Description)));
            }

            if (!await _users.IsInRoleAsync(user, role))
            {
                var result = await _users.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to add user to role: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
