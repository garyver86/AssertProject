using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpAmenitiesCategory
{
    public int AmenitiesCategoryId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<TpAmenitiesType> TpAmenitiesTypes { get; set; } = new List<TpAmenitiesType>();
}
