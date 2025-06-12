using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlCalendarDiscount
{
    public long CalendarDiscount { get; set; }

    public long CalendarId { get; set; }

    public decimal DiscountCalculated { get; set; }

    public decimal Porcentage { get; set; }

    public int DiscountTypeForTypePriceId { get; set; }

    public bool IsDiscount { get; set; }

    public virtual TlListingCalendar Calendar { get; set; } = null!;

    public virtual TDiscountTypeForTypePrice DiscountTypeForTypePrice { get; set; } = null!;
}
