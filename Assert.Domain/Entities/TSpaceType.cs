using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TSpaceType
{
    public int SpaceTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<TlListingSpace> TlListingSpaces { get; set; } = new List<TlListingSpace>();
}
