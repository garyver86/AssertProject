using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingViewHistory
{
    public long ListingViewHitoryId { get; set; }

    public int UserId { get; set; }

    public long ListingRentId { get; set; }

    public DateTime? ViewDate { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
