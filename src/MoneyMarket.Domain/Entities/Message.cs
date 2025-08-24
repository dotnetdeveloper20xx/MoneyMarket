using MoneyMarket.Domain.Common;

namespace MoneyMarket.Domain.Entities;

public class Message : AuditableEntity
{
    public Guid MessageId { get; private set; } = Guid.NewGuid();
    public Guid FromUserId { get; private set; }
    public Guid ToUserId { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }

    private Message() { }

    public Message(Guid fromUserId, Guid toUserId, string subject, string body)
    {
        if (fromUserId == Guid.Empty || toUserId == Guid.Empty) throw new ArgumentException("User IDs required.");
        FromUserId = fromUserId;
        ToUserId = toUserId;
        Subject = subject?.Trim() ?? "";
        Body = body?.Trim() ?? "";
    }

    public void MarkRead()
    {
        IsRead = true;
        Touch("recipient");
    }
}
