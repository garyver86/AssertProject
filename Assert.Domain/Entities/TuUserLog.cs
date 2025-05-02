namespace Assert.Domain.Entities;

public partial class TuUserLog
{
    public int UserLogId { get; set; }

    public int? UserId { get; set; }

    public int? UserRegId { get; set; }

    public DateTime? DateRegister { get; set; }

    public string? Command { get; set; }

    public string? JsonData { get; set; }

    public string? IpAddress { get; set; }

    public string? TypeLog { get; set; }

    public string? TableName { get; set; }

    public string? ColumnName { get; set; }

    public string? Ip { get; set; }

    public bool? IsMobile { get; set; }

    public string? BrowseInfo { get; set; }

    public string? ApplicationCode { get; set; }

    public virtual TuUser? User { get; set; }
}
