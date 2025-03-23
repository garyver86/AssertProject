namespace Assert.Domain.Entities;

public partial class TlListingDistanceRate
{
    public long ListingDistanceRateId { get; set; }

    public long ListingRentId { get; set; }

    public bool? IsAllow { get; set; }

    public decimal? Include { get; set; }

    public decimal? Overage { get; set; }

    public int? DistanceRateTypeId { get; set; }

    public virtual TDistanceRateType? DistanceRateType { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
