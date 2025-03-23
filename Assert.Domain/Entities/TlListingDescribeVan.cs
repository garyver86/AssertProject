namespace Assert.Domain.Entities;

public partial class TlListingDescribeVan
{
    public long ListingDescribeVanId { get; set; }

    public long? ListingRentId { get; set; }

    public string? Description { get; set; }

    public string? Included { get; set; }

    public string? Recomendation { get; set; }

    public string? ThingsDesription { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }
}
