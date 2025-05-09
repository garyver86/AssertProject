using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TimeZone1
{
    public int Id { get; set; }

    public string TimezoneId { get; set; } = null!;

    public string TimezoneLabel { get; set; } = null!;

    public string? TimezoneName { get; set; }

    public string? Diff { get; set; }

    public string? CountryCode { get; set; }

    public string? Abbreviation { get; set; }

    public int? TimeStart { get; set; }

    public int? GmtOffset { get; set; }

    public bool? Dst { get; set; }

    public virtual ICollection<TuUser> TuUsers { get; set; } = new List<TuUser>();
}
