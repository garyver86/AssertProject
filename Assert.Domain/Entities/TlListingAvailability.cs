namespace Assert.Domain.Entities;

public partial class TlListingAvailability
{
    public long ListingAvailabilityId { get; set; }

    public DateOnly? BlockedFrom { get; set; }

    public DateOnly? BlockedTo { get; set; }

    public long? ListingRentId { get; set; }

    public string? BlockedFromDescription { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public int? BlockTypeId { get; set; }

    public int? UserIdRenter { get; set; }

    public string? RenterName { get; set; }

    public long? BookId { get; set; }

    public virtual TAvailabilityBlockType? BlockType { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }
}
