namespace Assert.Domain.Entities;

public partial class TFeaturedAspectType
{
    public int FeaturedAspectType { get; set; }

    public string FeaturedAspectCode { get; set; } = null!;

    public string FeaturedAspectName { get; set; } = null!;

    public int FeaturedAspectStatus { get; set; }

    public virtual ICollection<TlListingFeaturedAspect> TlListingFeaturedAspects { get; set; } = new List<TlListingFeaturedAspect>();
}
