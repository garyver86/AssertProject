using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TLocation
{
    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public string? Details { get; set; }

    public bool? IsTop { get; set; }

    public string? UrlImage { get; set; }

    public int? CityId { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
