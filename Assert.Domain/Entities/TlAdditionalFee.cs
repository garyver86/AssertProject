using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlAdditionalFee
{
    public int AdditionalFeeId { get; set; }

    public string FeeCode { get; set; } = null!;

    public string DeeDescription { get; set; } = null!;

    public string CalculationType { get; set; } = null!;

    public decimal FeeValue { get; set; }

    public virtual ICollection<TlListingAdditionalFee> TlListingAdditionalFees { get; set; } = new List<TlListingAdditionalFee>();

    public virtual ICollection<TlListingCalendarAdditionalFee> TlListingCalendarAdditionalFees { get; set; } = new List<TlListingCalendarAdditionalFee>();
}
