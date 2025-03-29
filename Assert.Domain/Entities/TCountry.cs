using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCountry
{
    public long CountryId { get; set; }

    public string IataCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual ICollection<TState> TStates { get; set; } = new List<TState>();
}
