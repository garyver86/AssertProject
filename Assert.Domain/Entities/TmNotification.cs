namespace Assert.Domain.Entities;

public partial class TmNotification
{
    public long NotificationId { get; set; }

    public int UserId { get; set; }

    public string NotificationType { get; set; } = null!;

    public string? SubjectNotification { get; set; }

    public string MessageNotification { get; set; } = null!;

    public DateTime SentDate { get; set; }

    public string SendStatus { get; set; } = null!;

    public string? SendChannel { get; set; }

    public long? ReferenceId { get; set; }

    public string? ReferecceTable { get; set; }

    public virtual TuUser User { get; set; } = null!;
}
