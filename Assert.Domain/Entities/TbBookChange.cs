namespace Assert.Domain.Entities;

public partial class TbBookChange
{
    public long BookChangeId { get; set; }

    public long BookId { get; set; }

    public string ActionChange { get; set; } = null!;

    public DateTime DateTimeChange { get; set; }

    public string? IpAddress { get; set; }

    public string? ApplicationCode { get; set; }

    public bool? IsMobile { get; set; }

    public string? BrowserInfo { get; set; }

    public string? AdditionalData { get; set; }

    public int? UserId { get; set; }

    public virtual TbBook Book { get; set; } = null!;
}
