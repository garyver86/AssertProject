namespace Assert.Domain.Common;

public class LogQueueModel
{
    public Exception Exception { get; set; } = default!;
    public string Action { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public object Data { get; set; } = default!;
    public int? UserId { get; set; }
    public string? IpAddress { get; set; }
    public string BrowseInfo { get; set; } = string.Empty;
}
