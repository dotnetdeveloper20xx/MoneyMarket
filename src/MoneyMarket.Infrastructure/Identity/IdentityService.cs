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
        {
            _users = users;
            _signIn = signIn;
            _roles = roles;
            _jwt = jwt;
        }

        public async Task<(bool Succeeded, string? UserId, string[] Errors)> RegisterAsync(string email, string password, string role)
        {
            var user = new ApplicationUser { UserName = email, Email = email, EmailConfirmed = true };
            var create = await _users.CreateAsync(user, password);
            if (!create.Succeeded)
                return (false, null, create.Errors.Select(e => e.Description).ToArray());

            if (!await _roles.RoleExistsAsync(role))
                await _roles.CreateAsync(new ApplicationRole { Name = role });

            var addRole = await _users.AddToRoleAsync(user, role);
            if (!addRole.Succeeded)
                return (false, null, addRole.Errors.Select(e => e.Description).ToArray());

            return (true, user.Id.ToString(), Array.Empty<string>());
        }

        public async Task<(bool Succeeded, string Token, string[] Errors)> LoginAsync(string email, string password)
        {
            var user = await _users.FindByEmailAsync(email);
            if (user is null)
                return (false, "", new[] { "User not found" });

            var ok = await _signIn.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
            if (!ok.Succeeded)
                return (false, "", new[] { "Invalid credentials" });

            var roles = await _users.GetRolesAsync(user);
            // Important: ensure CreateToken embeds NameIdentifier/sub with user.Id (string)
            var token = _jwt.CreateToken(user.Id.ToString(), email, roles);
            return (true, token, Array.Empty<string>());
        }

        public async Task AddUserToRoleAsync(Guid userId, string role, CancellationToken ct)
        {
            // UserManager APIs do not accept CancellationToken; we ignore 'ct' here.
            var user = await _users.FindByIdAsync(userId.ToString())
                       ?? throw new InvalidOperationException("User not found.");

            if (!await _roles.RoleExistsAsync(role))
            {
                var created = await _roles.CreateAsync(new ApplicationRole { Name = role });
                if (!created.Succeeded)
                    throw new InvalidOperationException("Failed to create role: " +
                        string.Join(", ", created.Errors.Select(e => e.Description)));
            }

            if (!await _users.IsInRoleAsync(user, role))
            {
                var result = await _users.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                    throw new InvalidOperationException("Failed to add user to role: " +
                        string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<string> EnsureUserExistsAsync(Guid userId, CancellationToken ct)
        {
            var user = await _users.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new InvalidOperationException($"Identity user not found for id '{userId}' in dbo.ApplicationUser.");
            return user.Id!.ToString();
        }

        public async Task<Guid> GetUserIdGuidByEmailAsync(string email, CancellationToken ct)
        {
            var user = await _users.FindByEmailAsync(email)
                       ?? throw new InvalidOperationException($"Identity user not found for email '{email}'.");

            // Handle both Guid-key and string-key identity schemas
            var idObj = (object)user.Id!;
            if (idObj is Guid gid) return gid;
            if (idObj is string sid && Guid.TryParse(sid, out var parsed)) return parsed;

            throw new InvalidOperationException(
                $"Unsupported ApplicationUser.Id type '{idObj.GetType().Name}'. Expected Guid or Guid-formatted string.");
        }
    }
}
