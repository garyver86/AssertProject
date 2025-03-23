using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TState
{
    public long StateId { get; set; }

    public long CountryId { get; set; }

    public string? IataCode { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual TCountry Country { get; set; } = null!;

    public virtual ICollection<TCity> TCities { get; set; } = new List<TCity>();

    public virtual ICollection<TCounty> TCounties { get; set; } = new List<TCounty>();

    public virtual ICollection<TuAddress> TuAddresses { get; set; } = new List<TuAddress>();
}
