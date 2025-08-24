using MoneyMarket.Domain.Common;
using MoneyMarket.Domain.Enums;

namespace MoneyMarket.Domain.Entities;

public class Notification : AuditableEntity
{
    public Guid NotificationId { get; private set; } = Guid.NewGuid();
    public Guid RecipientUserId { get; private set; }
    public NotificationType Type { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public bool IsRead { get; private set; }

    private Notification() { }

    public Notification(Guid recipientUserId, NotificationType type, string title, string message)
    {
        if (recipientUserId == Guid.Empty) throw new ArgumentException("Recipient required.");
        RecipientUserId = recipientUserId;
        Type = type;
        Title = title?.Trim() ?? "";
        Message = message?.Trim() ?? "";
    }

    public void MarkRead()
    {
        IsRead = true;
        Touch("recipient");
    }
}
