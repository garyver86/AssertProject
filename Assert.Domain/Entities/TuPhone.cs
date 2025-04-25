namespace Assert.Domain.Entities;

public partial class TuPhone
{
    public int PhoneId { get; set; }

    public int? UserId { get; set; }

    public string? CountryCode { get; set; }

    public string? AreaCode { get; set; }

    public string? Number { get; set; }

    public bool? IsPrimary { get; set; }

    public bool? IsMobile { get; set; }

    public int? Status { get; set; }

    public virtual TuUser? User { get; set; }
}
