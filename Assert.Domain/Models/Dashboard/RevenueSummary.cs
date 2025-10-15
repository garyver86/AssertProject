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
}
