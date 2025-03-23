using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TQuickTipType
{
    public int QuickTipTypeId { get; set; }

    public string DescriptionType { get; set; } = null!;

    public virtual ICollection<TQuickTip> TQuickTips { get; set; } = new List<TQuickTip>();
}
