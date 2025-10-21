using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models.Dashboard
{
    public class RevenueSummary
    {
        public decimal GrandTotalRevenue { get; set; }
        public decimal GrandTotalPlatformFees { get; set; }
        public decimal GrandTotalNetRevenue { get; set; }
        public int GrandTotalTransactions { get; set; }
        public List<RevenueReport> MonthlyData { get; set; } = new();
    }

    public class RevenueReport
    {
        public int Month { get; set; }
        public int Year { get; set; }


        public decimal TotalRevenue { get; set; }
        public decimal TotalPlatformFees { get; set; }
        public decimal NetRevenue { get; set; }
        public int TransactionCount { get; set; }


        public string MonthSpanish =>
            new DateTime(Year, Month, 1).ToString("MMMM", new CultureInfo("es-ES"));
        public string MonthEnglish =>
            new DateTime(Year, Month, 1).ToString("MMMM", new CultureInfo("en-US"));
    }

    public class BusinessReport
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Ingresos
        public decimal ConfirmedRevenue { get; set; }     // Ingresos ya pagados
        public decimal FutureRevenue { get; set; }        // Reservas confirmadas por cobrar
        public decimal AssertFee { get; set; }         // Ingreso Assert (comisiones)

        // Reservas
        public int ConfirmedBookings { get; set; }        // Reservas confirmadas
        public int CancelledBookings { get; set; }        // Reservas canceladas

        // Ocupación
        public int OccupiedNights { get; set; }           // Noches ocupadas
        public decimal OccupancyRate { get; set; }        // Factor de ocupación (%)

        public int Month { get; set; }
        public int Year { get; set; }


        public string MonthSpanish =>
            new DateTime(Year, Month, 1).ToString("MMMM", new CultureInfo("es-ES"));
        public string MonthEnglish =>
            new DateTime(Year, Month, 1).ToString("MMMM", new CultureInfo("en-US"));
    }
}
