namespace Assert.Application.DTOs.Requests;

public class EmailNotificationRequestDTO
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
