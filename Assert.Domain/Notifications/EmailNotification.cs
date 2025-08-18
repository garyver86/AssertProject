namespace Assert.Domain.Notifications;

public class EmailNotification
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
