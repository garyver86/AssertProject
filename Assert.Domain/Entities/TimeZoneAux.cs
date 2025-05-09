using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TimeZoneAux
{
    public string ConeName { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public double TimeStart { get; set; }

    public string GmtOffset { get; set; } = null!;

    public string Dst { get; set; } = null!;
}
