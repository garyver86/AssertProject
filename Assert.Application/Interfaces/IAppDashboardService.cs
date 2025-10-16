using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;

namespace Assert.Application.Interfaces
{
    public interface IAppDashboardService
    {
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportAsync(RevenueReportRequestDTO request);
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByYearAsync(int year);
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByUserAsync(RevenueReportRequestDTO request, int userId);
        Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByYearAndUserAsync(int year, int userId);
        Task<ReturnModelDTO<DashboardInfoDTO>> GetDashboardInfo(int year, int? month);
        Task<ReturnModelDTO<ListingRentRankingDTO>> GetPropertyRanking(long hostId,
            DateTime startDate, DateTime endDate);
    }
}
