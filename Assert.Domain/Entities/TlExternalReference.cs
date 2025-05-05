namespace Assert.Domain.Entities;

public partial class TlExternalReference
{
    public long ExternalReferenceId { get; set; }

    public long ListingRentId { get; set; }

    public int BookingPlatformId { get; set; }

    public bool IsEnabled { get; set; }

    public string ExternalIdentifier { get; set; } = null!;

    public virtual TBookingPlatform BookingPlatform { get; set; } = null!;

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
