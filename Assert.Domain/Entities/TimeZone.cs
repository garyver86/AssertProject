namespace Assert.Domain.Entities;

public partial class TimeZone
{
    public int TimeZone1 { get; set; }

    public string ZoneName { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string Abbreviation { get; set; } = null!;

    public int TimeStart { get; set; }

    public int GmtOffset { get; set; }

    public string Dst { get; set; } = null!;
}
