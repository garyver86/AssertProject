using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingAdditionalFee
{
    public long ListingAdditionalFeeId { get; set; }

    public long ListingRentId { get; set; }

    public decimal AmountFee { get; set; }

    public bool IsPercent { get; set; }

    public int AdditionalFeeId { get; set; }

    public virtual TlAdditionalFee AdditionalFee { get; set; } = null!;

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
