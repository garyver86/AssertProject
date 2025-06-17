using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCalendarBlockType
{
    public byte CalendarBlockTypeId { get; set; }

    public string BlockCode { get; set; } = null!;

    public string BlockTypeName { get; set; } = null!;

    public virtual ICollection<TlListingCalendar> TlListingCalendars { get; set; } = new List<TlListingCalendar>();
}
