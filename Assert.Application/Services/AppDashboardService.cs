using Assert.Application.DTOs.Requests;
using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Models.Dashboard;
using Assert.Domain.Services;
using AutoMapper;
using Azure.Core;

namespace Assert.Application.Services
{
    public class AppDashboardService : IAppDashboardService
    {
        private readonly IDashboardService _dashboardService;
        private readonly IMapper _mapper;
        private readonly RequestMetadata _metadata;
        public AppDashboardService(IDashboardService dashboardService, IMapper mapper, RequestMetadata metadata)
        {
            _dashboardService = dashboardService;
            _mapper = mapper;
            _metadata = metadata;
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
    }
}
