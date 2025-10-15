using System.Globalization;

namespace Assert.Domain.Models.Dashboard;

public class DashboardInfo
{
    public int? Month { get; set; } = DateTime.Now.Month;
    public int Year { get; set; } = DateTime.Now.Year;

    public string? MonthSpanish =>
    Month.HasValue
        ? new DateTime(Year, Month.Value, 1).ToString("MMMM", new CultureInfo("es-ES"))
        : null;

    public string? MonthEnglish =>
        Month.HasValue
            ? new DateTime(Year, Month.Value, 1).ToString("MMMM", new CultureInfo("en-US"))
            : null;

    public FilterMode FilterMode { get; set; }

    public decimal TotalPaid { get; set; }
    public List<MetricEntry> PaidByPeriod { get; set; } = [];

    public decimal TotalUpcoming { get; set; }
    public List<MetricEntry> UpcomingByPeriod { get; set; } = [];

    public int TotalConfirmedBooks { get; set; }
    public List<MetricEntryInt> ConfirmedBooksByPeriod { get; set; } = [];

    public int TotalNightsBooked { get; set; }
    public List<MetricEntryInt> NightsBookedByPeriod { get; set; } = [];

    public decimal AverageNightsPerBook { get; set; }
    public List<MetricEntry> AverageNightsPerBookByPeriod { get; set; } = [];


}

public record MetricEntry(int Period, decimal Value);
public record MetricEntryInt(int Period, int Value);   

public enum FilterMode
{
    YearAndMonth,
    Year,
}

