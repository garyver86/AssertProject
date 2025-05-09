using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpSecurityItemType
{
    public int SecurityItemTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? IconLink { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<TlListingSecurityItem> TlListingSecurityItems { get; set; } = new List<TlListingSecurityItem>();
}
