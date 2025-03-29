using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCity
{
    public long CityId { get; set; }

    public string Name { get; set; } = null!;

    public long CountyId { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual TCounty County { get; set; } = null!;

    public virtual ICollection<TpPropertyAddress> TpPropertyAddresses { get; set; } = new List<TpPropertyAddress>();
}
