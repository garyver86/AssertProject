using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingCalendarAdditionalFee
{
    public long ListingCalendarAdditionalFeeId { get; set; }

    public long CalendarId { get; set; }

    public decimal AmountFee { get; set; }

    public int AdditionalFeeId { get; set; }

    public bool IsPercent { get; set; }

    public virtual TlAdditionalFee AdditionalFee { get; set; } = null!;

    public virtual TlListingCalendar Calendar { get; set; } = null!;
}
