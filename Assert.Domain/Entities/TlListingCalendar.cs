using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingCalendar
{
    public long CalendarId { get; set; }

    public long ListingrentId { get; set; }

    public DateOnly Date { get; set; }

    public byte? BlockType { get; set; }

    public decimal? Price { get; set; }

    public string? BlockReason { get; set; }

    public long? BookId { get; set; }

    public int? MinimumStay { get; set; }

    public int? MaximumStay { get; set; }

    public int? MinimumNotice { get; set; }

    public TimeOnly? MinimumNoticeHour { get; set; }

    public int? PreparationDays { get; set; }

    public int? AvailabilityWindowMonth { get; set; }

    public string? CheckInDays { get; set; }

    public string? CheckOutDays { get; set; }

    public virtual TCalendarBlockType? BlockTypeNavigation { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent Listingrent { get; set; } = null!;

    public virtual ICollection<TlCalendarDiscount> TlCalendarDiscounts { get; set; } = new List<TlCalendarDiscount>();
}
