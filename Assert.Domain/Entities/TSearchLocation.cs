using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TSearchLocation
{
    public string SearchValue { get; set; } = null!;

    public string? JsonResultLocation { get; set; }

    public string? JsonCitiesResult { get; set; }

    public string? JsonCountiesResult { get; set; }

    public string? JsonStatesResult { get; set; }

    public string? JsonCountriesResult { get; set; }
}
