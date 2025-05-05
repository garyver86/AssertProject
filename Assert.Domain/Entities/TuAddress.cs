namespace Assert.Domain.Entities;

public partial class TuAddress
{
    public int AddressId { get; set; }

    public int? UserId { get; set; }

    public string? ZipCode { get; set; }

    public int? StateId { get; set; }

    public string? City { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public bool? IsCurrent { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public virtual TState? State { get; set; }

    public virtual TuUser? User { get; set; }
}
