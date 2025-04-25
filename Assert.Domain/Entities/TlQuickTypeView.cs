namespace Assert.Domain.Entities;

public partial class TlQuickTypeView
{
    public int QuickTipViewId { get; set; }

    public int QuickTipId { get; set; }

    public int ViewTypeId { get; set; }

    public int Status { get; set; }

    public virtual TQuickTip QuickTip { get; set; } = null!;

    public virtual TlViewType ViewType { get; set; } = null!;
}
