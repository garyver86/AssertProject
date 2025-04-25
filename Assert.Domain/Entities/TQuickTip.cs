namespace Assert.Domain.Entities;

public partial class TQuickTip
{
    public int QuickTipId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? IconLink { get; set; }

    public int? QuickTipTypeId { get; set; }

    public string? DisplayElement { get; set; }

    public virtual TQuickTipType? QuickTipType { get; set; }

    public virtual ICollection<TlQuickTypeView> TlQuickTypeViews { get; set; } = new List<TlQuickTypeView>();
}
