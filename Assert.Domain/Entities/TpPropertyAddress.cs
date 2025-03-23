using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpPropertyAddress
{
    public long PropertyAddressId { get; set; }

    public long? PropertyId { get; set; }

    public string? ZipCode { get; set; }

    public long? CityId { get; set; }

    public string? Address1 { get; set; }

    public string? Address2 { get; set; }

    public virtual TCity? City { get; set; }

    public virtual TpProperty? Property { get; set; }
}
