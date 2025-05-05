namespace Assert.Domain.Entities;

public partial class TlListingStepsView
{
    public int ListngStepsViewId { get; set; }

    public long? ListingStepsId { get; set; }

    public int? ViewTypeId { get; set; }

    public string? Mode { get; set; }

    public bool? IsEnded { get; set; }

    public virtual TlListingStep? ListingSteps { get; set; }

    public virtual TlViewType? ViewType { get; set; }
}
