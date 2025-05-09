using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingSecurityItem
{
    public long ListingSecurityItemId { get; set; }

    public long? ListingRentId { get; set; }

    public int? SecurityItemTypeId { get; set; }

    public bool? Value { get; set; }

    public string? ValueString { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TpSecurityItemType? SecurityItemType { get; set; }
}
