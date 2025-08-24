namespace MoneyMarket.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public string? CreatedBy { get; protected set; }
    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    public string? LastModifiedBy { get; protected set; }
    public DateTime? LastModifiedAtUtc { get; protected set; }

    public void Touch(string? actor)
    {
        LastModifiedBy = actor;
        LastModifiedAtUtc = DateTime.UtcNow;
    }
}
