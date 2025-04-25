namespace Assert.Domain.Entities;

public partial class TlListingSpace
{
    public long ListingSpaceId { get; set; }

    public int Value1 { get; set; }

    public int SpaceTypeId { get; set; }

    public long ListingId { get; set; }

    public virtual TlListingRent Listing { get; set; } = null!;

    public virtual TSpaceType SpaceType { get; set; } = null!;
}
