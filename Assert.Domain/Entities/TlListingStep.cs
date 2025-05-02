namespace Assert.Domain.Entities;

public partial class TlListingStep
{
    public long ListingStepsId { get; set; }

    public long? ListingRentId { get; set; }

    public int? StepsTypeId { get; set; }

    public int? ListingStepsStatusId { get; set; }

    public string? AditionalDetail { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TlListingStepsStatus? ListingStepsStatus { get; set; }

    public virtual TlStepsType? StepsType { get; set; }

    public virtual ICollection<TlListingStepsView> TlListingStepsViews { get; set; } = new List<TlListingStepsView>();
}
