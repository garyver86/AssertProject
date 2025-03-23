using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpProperty
{
    public long PropertyId { get; set; }

    public long? ListingRentId { get; set; }

    public int? PropertySubtypeId { get; set; }

    public string? ExternalReference { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TpPropertySubtype? PropertySubtype { get; set; }

    public virtual ICollection<TpPropertyAddress> TpPropertyAddresses { get; set; } = new List<TpPropertyAddress>();
}
