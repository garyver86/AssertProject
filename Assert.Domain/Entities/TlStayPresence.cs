using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlStayPresence
{
    public int StayPresenceId { get; set; }

    public long ListingRentId { get; set; }

    public int StayPrecenseTypeId { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;

    public virtual TStayPresenceType StayPrecenseType { get; set; } = null!;
}
