using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class MethodOfPaymentService : IMethodOfPaymentService
    {
        private readonly IErrorHandler _errorHandler;
        private readonly IMethodOfPaymentRepository _methodOfPaymentRepository;
        public MethodOfPaymentService(IErrorHandler errorHandler, IMethodOfPaymentRepository methodOfPaymentRepository)
        {
            _errorHandler = errorHandler;
            _methodOfPaymentRepository = methodOfPaymentRepository;
        }

        public async Task<ReturnModel<List<PayMethodOfPayment>>> GetAllAsync(int countryId)
        {
            ReturnModel<List<PayMethodOfPayment>> result = new ReturnModel<List<PayMethodOfPayment>>();
            try
            {
                var result_data = await _methodOfPaymentRepository.GetAllAsync(countryId);
                return new ReturnModel<List<PayMethodOfPayment>>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false,
                    Data = result_data
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.DisableHostRole", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<PayMethodOfPayment>> GetByIdAsync(int id)
        {
            ReturnModel<PayMethodOfPayment> result = new ReturnModel<PayMethodOfPayment>();
            try
            {
                var result_data = await _methodOfPaymentRepository.GetByIdAsync(id);
                return new ReturnModel<PayMethodOfPayment>
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("UserService.DisableHostRole", ex, null, true);
            }
            return result;
        }
    }
}
