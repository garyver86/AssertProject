using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingAmenity
{
    public long ListingAmenitiesId { get; set; }

    public long ListingRentId { get; set; }

    public int AmenitiesTypeId { get; set; }

    public bool? Value { get; set; }

    public string? ValueString { get; set; }

    public bool IsPremium { get; set; }

    public virtual TpAmenitiesType AmenitiesType { get; set; } = null!;

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
