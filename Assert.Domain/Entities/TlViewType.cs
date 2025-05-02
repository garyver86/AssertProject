namespace Assert.Domain.Entities;

public partial class TlViewType
{
    public int ViewTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? StepTypeId { get; set; }

    public int? ViewTypeIdParent { get; set; }

    public int? NextViewTypeId { get; set; }

    public virtual TlStepsType? StepType { get; set; }

    public virtual ICollection<TlListingStepsView> TlListingStepsViews { get; set; } = new List<TlListingStepsView>();

    public virtual ICollection<TlQuickTypeView> TlQuickTypeViews { get; set; } = new List<TlQuickTypeView>();
}
