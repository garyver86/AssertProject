using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses;

public class DashboardInfoDTO
{
    public int? Month { get; set; }
    public int Year { get; set; }

    public string? MonthSpanish { get; set; }
    public string? MonthEnglish { get; set; }

    public FilterModeDto FilterMode { get; set; }

    public decimal TotalPaid { get; set; }
    public List<MetricEntryDTO> PaidByPeriod { get; set; } = [];

    public decimal TotalUpcoming { get; set; }
    public List<MetricEntryDTO> UpcomingByPeriod { get; set; } = [];

    public int TotalConfirmedBooks { get; set; }
    public List<MetricEntryIntDTO> ConfirmedBooksByPeriod { get; set; } = [];

    public int TotalNightsBooked { get; set; }
    public List<MetricEntryIntDTO> NightsBookedByPeriod { get; set; } = [];

    public decimal AverageNightsPerBook { get; set; }
    public List<MetricEntryDTO> AverageNightsPerBookByPeriod { get; set; } = [];

}
public record MetricEntryDTO(int Period, decimal Value);
public record MetricEntryIntDTO(int Period, int Value);

public enum FilterModeDto
{
    YearAndMonth,
    Year
}

