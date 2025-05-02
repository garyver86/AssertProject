namespace Assert.Domain.Entities;

public partial class TlListingFeaturedAspect
{
    public long ListingFeaturedAspectId { get; set; }

    public long ListingRentId { get; set; }

    public int FeaturesAspectTypeId { get; set; }

    public string? FeaturedAspectValue { get; set; }

    public virtual TFeaturedAspectType FeaturesAspectType { get; set; } = null!;

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
