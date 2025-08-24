namespace MoneyMarket.Domain.Entities;

public class User
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty; // TODO: replace with Identity hashing
    public string Role { get; set; } = "Borrower";
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
