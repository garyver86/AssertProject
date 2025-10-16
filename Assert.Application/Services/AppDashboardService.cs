using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models;
using Assert.Domain.Models.Dashboard;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppDashboardService : IAppDashboardService
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        private readonly RequestMetadata _metadata;
        private readonly IBookRepository _bookRespository;
        public AppDashboardService(IDashboardService dashboardService, 
            IMapper mapper, RequestMetadata metadata,
            IBookRepository bookRespository)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
            _metadata = metadata;
            _bookRespository = bookRespository;
        }
        public async Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportAsync(RevenueReportRequestDTO request)
        {
            RevenueReportRequest _request = _mapper.Map<RevenueReportRequest>(request);
            _request.UserId = _metadata.UserId;
            var result = await _dashboardService.GetRevenueReportAsync(_request);
            return new ReturnModelDTO<RevenueSummaryDTO>
            {
                Data = _mapper.Map<RevenueSummaryDTO>(result.Data),
                HasError = result.HasError,
                StatusCode = result.StatusCode,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError)
            };
        }

        public async Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByYearAsync(int year)
        {
            var result = await _dashboardService.GetRevenueReportByYearAsync(year, _metadata.UserId);
            return new ReturnModelDTO<RevenueSummaryDTO>
            {
                Data = _mapper.Map<RevenueSummaryDTO>(result.Data),
                HasError = result.HasError,
                StatusCode = result.StatusCode,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError)
            };
        }
        public async Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByUserAsync(RevenueReportRequestDTO request, int userId)
        {
            RevenueReportRequest _request = _mapper.Map<RevenueReportRequest>(request);
            _request.UserId = userId;
            var result = await _dashboardService.GetRevenueReportAsync(_request);
            return new ReturnModelDTO<RevenueSummaryDTO>
            {
                Data = _mapper.Map<RevenueSummaryDTO>(result.Data),
                HasError = result.HasError,
                StatusCode = result.StatusCode,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError)
            };
        }

        public async Task<ReturnModelDTO<RevenueSummaryDTO>> GetRevenueReportByYearAndUserAsync(int year, int userId)
        {
            var result = await _dashboardService.GetRevenueReportByYearAsync(year, userId);
            return new ReturnModelDTO<RevenueSummaryDTO>
            {
                Data = _mapper.Map<RevenueSummaryDTO>(result.Data),
                HasError = result.HasError,
                StatusCode = result.StatusCode,
                ResultError = _mapper.Map<ErrorCommonDTO>(result.ResultError)
            };
        }

        public async Task<ReturnModelDTO<DashboardInfoDTO>> GetDashboardInfo(int year, int? month)
        {
            var dashboardInfo = await _bookRespository.GetDashboardInfo(year, month);
            var dashboardInfoDto = _mapper.Map<DashboardInfoDTO>(dashboardInfo);

            return new ReturnModelDTO<DashboardInfoDTO>
            {
                Data = dashboardInfoDto,
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        public async Task<ReturnModelDTO<ListingRentRankingDTO>> GetPropertyRanking(long hostId,
            DateTime startDate, DateTime endDate)
        {
            var propertyRankingList = await _bookRespository.GetListingRentRankingAsync(hostId, startDate, endDate);
            var propertyRankingDtoList = _mapper.Map<ListingRentRankingDTO>(propertyRankingList);

            return new ReturnModelDTO<ListingRentRankingDTO>
            {
                Data = propertyRankingDtoList,
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }
    }
}
