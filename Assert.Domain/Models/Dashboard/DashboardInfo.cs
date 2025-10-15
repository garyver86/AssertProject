using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public decimal UpcomingPaymentsTotal { get; set; }
    public List<(int, decimal)> UpcomingPaymentsByMonth { get; set; } = [];
    public int TotalReservations { get; set; }
    public List<(int, int)> ReservationsByMonth { get; set; } = [];
    public int TotalNightsReserved { get; set; }
    public List<(int, int)> NightsReservedByMonth { get; set; } = [];
    public decimal AverageNightRate { get; set; }
    public List<(int, decimal)> AverageNightRateByMonth { get; set; } = [];

}

public enum FilterMode
{
    YearAndMonth,
    Year,
}

