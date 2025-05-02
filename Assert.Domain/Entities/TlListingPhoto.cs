namespace Assert.Domain.Entities;

public partial class TlListingPhoto
{
    public long ListingPhotoId { get; set; }

    public long? ListingRentId { get; set; }

    public string? PhotoLink { get; set; }

    public bool? IsPrincipal { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Position { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }
}
