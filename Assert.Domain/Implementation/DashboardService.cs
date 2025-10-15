using Assert.Domain.Models;
using Assert.Domain.Models.Dashboard;
using Assert.Domain.Repositories;
using Assert.Domain.Services;

namespace Assert.Domain.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IPayPriceCalculationRepository _payPriceCalculationRepository;
        private readonly IErrorHandler _errorHandler;
        public DashboardService(IPayPriceCalculationRepository payPriceCalculationRepository, IErrorHandler errorHandler)
        {
            _payPriceCalculationRepository = payPriceCalculationRepository;
            _errorHandler = errorHandler;
        }
        public async Task<ReturnModel<RevenueSummary>> GetRevenueReportAsync(RevenueReportRequest request)
        {
            ReturnModel<RevenueSummary> result = new ReturnModel<RevenueSummary>();
            try
            {

                var result_data = await _payPriceCalculationRepository.GetRevenueReportAsync(request);
                return new ReturnModel<RevenueSummary>
                {
                    StatusCode = ResultStatusCode.OK,
                    Data = result_data,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("DashboardService.GetRevenueReportAsync", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<RevenueSummary>> GetRevenueReportByYearAsync(int year, int userId)
        {
            ReturnModel<RevenueSummary> result = new ReturnModel<RevenueSummary>();
            try
            {
                var request = new RevenueReportRequest { Year = year, UserId = userId };
                var result_data = await _payPriceCalculationRepository.GetRevenueReportAsync(request);
                return new ReturnModel<RevenueSummary>
                {
                    StatusCode = ResultStatusCode.OK,
                    Data = result_data,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("DashboardService.GetRevenueReportByYearAsync", ex, null, true);
            }
            return result;
        }
    }
}
