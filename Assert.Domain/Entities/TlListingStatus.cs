using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingStatus
{
    public int ListingStatusId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TlListingRent> TlListingRents { get; set; } = new List<TlListingRent>();
}
