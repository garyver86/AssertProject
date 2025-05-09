using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingRentChange
{
    public long ListingRentChangesId { get; set; }

    public long ListingRentId { get; set; }

    public string ActionChange { get; set; } = null!;

    public DateTime DateTimeChange { get; set; }

    public string IpAddress { get; set; } = null!;

    public string? ApplicationCode { get; set; }

    public bool? IsMobile { get; set; }

    public string? BrowserInfoInfo { get; set; }

    public string? AdditionalData { get; set; }

    public int? UserId { get; set; }

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
