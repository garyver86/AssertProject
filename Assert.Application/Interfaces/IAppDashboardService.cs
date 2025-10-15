using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Domain.Models;
using Assert.Domain.Models.Dashboard;

namespace Assert.Application.Interfaces
{
    public interface IAppDashboardService
    {
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportAsync(RevenueReportRequestDTO request);
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByYearAsync(int year);
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByUserAsync(RevenueReportRequestDTO request, int userId);
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByYearAndUserAsync(int year, int userId);
    }
}
