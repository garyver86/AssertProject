using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Common.Metadata;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Assert.Shared.Extensions;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppMethodOfPaymentService : IAppMethodOfPaymentService
    {
        private readonly IMethodOfPaymentService _mopService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly RequestMetadata _metadata;

        public AppMethodOfPaymentService(IMethodOfPaymentService methodOfPaymentService, IMapper mapper, IErrorHandler errorHandler,
            IExceptionLoggerService exceptionLoggerService, RequestMetadata metadata)
        {
            _mopService = methodOfPaymentService;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _exceptionLoggerService = exceptionLoggerService;
            _metadata = metadata;
        }

        public async Task<ReturnModelDTO<List<PayMethodOfPaymentDTO>>> GetAllAsync(int countryId)
        {
            try
            {
                var mops = await _mopService.GetAllAsync(countryId);

                return new ReturnModelDTO<List<PayMethodOfPaymentDTO>>
                {
                    Data = _mapper.Map<List<PayMethodOfPaymentDTO>>(mops.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { countryId });

                return new ReturnModelDTO<List<PayMethodOfPaymentDTO>>
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.InternalError,
                    ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMethodOfPaymentService.GetAllAsync", ex, new { countryId }, true))
                };
            }
        }

        public async Task<ReturnModelDTO<PayMethodOfPaymentDTO>> GetByIdAsync(int id)
        {
            try
            {
                var mop = await _mopService.GetByIdAsync(id);

                return new ReturnModelDTO<PayMethodOfPaymentDTO>
                {
                    Data = _mapper.Map<PayMethodOfPaymentDTO>(mop.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, new { id });

                return new ReturnModelDTO<PayMethodOfPaymentDTO>
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.InternalError,
                    ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetErrorException("AppMethodOfPaymentService.GetByIdAsync", ex, new { id }, true))
                };
            }
        }
    }
}
