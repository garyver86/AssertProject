namespace Assert.Domain.Entities;

public partial class TlStepsType
{
    public int StepsTypeId { get; set; }

    public string? CodeParent { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Number { get; set; }

    public int? Status { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TlListingStep> TlListingSteps { get; set; } = new List<TlListingStep>();

    public virtual ICollection<TlViewType> TlViewTypes { get; set; } = new List<TlViewType>();
}
