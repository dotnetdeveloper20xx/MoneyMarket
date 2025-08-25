namespace MoneyMarket.Application.Common.Abstractions;

public interface IJwtTokenService
{
    string CreateToken(string userId, string email, IEnumerable<string> roles, IDictionary<string, string>? customClaims = null);
}
