namespace Assert.Domain.Entities;

public partial class TpAmenitiesType
{
    public int AmenitiesTypeId { get; set; }

    public int? AmenitiesCategoryId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? IconLink { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual TpAmenitiesCategory? AmenitiesCategory { get; set; }

    public virtual ICollection<TlListingAmenity> TlListingAmenities { get; set; } = new List<TlListingAmenity>();
}
