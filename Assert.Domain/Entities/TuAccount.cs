namespace Assert.Domain.Entities;

public partial class TuAccount
{
    public long AccountId { get; set; }

    public int? UserId { get; set; }

    public string? Password { get; set; }

    public int? IncorrectAccess { get; set; }

    public bool? IsBlocked { get; set; }

    public DateTime? LastBlockDate { get; set; }

    public DateTime? DateLastLogin { get; set; }

    public string? IpLastLogin { get; set; }

    public string? Status { get; set; }

    public DateTime? TemporaryBlockTo { get; set; }

    public bool? ForceChange { get; set; }

    public virtual TuUser? User { get; set; }
}
