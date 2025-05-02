namespace Assert.Domain.Common;

public class RequestMetadata
{
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IsMobile { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;

    //from login
    public string UserName { get; set; }
    public int UserId { get; set; }
    public int AccountId { get; set; }
}
