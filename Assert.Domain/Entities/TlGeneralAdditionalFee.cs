using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlGeneralAdditionalFee
{
    public long GeneralAdditionalFeeId { get; set; }

    public int UserId { get; set; }

    public decimal AmountFee { get; set; }

    public bool IsPercent { get; set; }

    public int AdditionalFeeId { get; set; }

    public virtual TlAdditionalFee AdditionalFee { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
