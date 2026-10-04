using FixFlow.Api.Common.Domain;

namespace FixFlow.Api.Modules.Notifications.Domain;

public enum NotificationChannel
{
    InApp = 1,
    Email = 2,
    Sms = 3
}

public class Notification : Entity<Guid>, IAggregateRoot
{
    public Guid RecipientUserId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Message { get; private set; } = string.Empty;
    public NotificationChannel Channel { get; private set; } = NotificationChannel.InApp;
    public bool IsRead { get; private set; }
    public DateTime? ReadAtUtc { get; private set; }
    public string? ActionUrl { get; private set; }

    private Notification() { }

    public Notification(Guid id, Guid recipientUserId, string title, string message, string? actionUrl = null) : base(id)
    {
        RecipientUserId = recipientUserId;
        Title = title;
        Message = message;
        ActionUrl = actionUrl;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
        ReadAtUtc = DateTime.UtcNow;
        MarkUpdated();
    }
}
