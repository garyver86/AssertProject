namespace Assert.Domain.Entities;

public partial class TAvailabilityBlockType
{
    public int AvailabilityBlockTypeId { get; set; }

    public string BlockTypeCode { get; set; } = null!;

    public string BlockTypeDescription { get; set; } = null!;

    public string? Color { get; set; }

    public virtual ICollection<TlListingAvailability> TlListingAvailabilities { get; set; } = new List<TlListingAvailability>();
}
