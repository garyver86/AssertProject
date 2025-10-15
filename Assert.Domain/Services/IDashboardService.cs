using Assert.Domain.Models;
using Assert.Domain.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IDashboardService
    {
        Task<ReturnModel<RevenueSummary>> GetRevenueReportAsync(RevenueReportRequest request);
        Task<ReturnModel<RevenueSummary>> GetRevenueReportByYearAsync(int year, int userId);
    }
}
