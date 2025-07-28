using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCountry
{
    public int CountryId { get; set; }

    public string IataCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual ICollection<PayCountryConfiguration> PayCountryConfigurations { get; set; } = new List<PayCountryConfiguration>();

    public virtual ICollection<PayTransaction> PayTransactions { get; set; } = new List<PayTransaction>();

    public virtual ICollection<TState> TStates { get; set; } = new List<TState>();
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
