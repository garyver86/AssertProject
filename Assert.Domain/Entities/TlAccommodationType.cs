using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlAccommodationType
{
    public int AccommodationTypeId { get; set; }

    public string AccommodationName { get; set; } = null!;

    public string? AccommodationDescription { get; set; }

    public string? AccomodationCode { get; set; }

    public string? AccommodationIcon { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<TlListingRent> TlListingRents { get; set; } = new List<TlListingRent>();
}
