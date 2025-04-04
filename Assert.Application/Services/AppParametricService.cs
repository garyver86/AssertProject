using Assert.Application.DTOs;
using Assert.Application.Interfaces;
using Assert.Domain.Models;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppParametricService : IAppParametricService
    {

        private readonly IParametricService _parametricService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppParametricService(IParametricService parametricService, IErrorHandler errorHandler, IMapper mapper)
        {
            _parametricService = parametricService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }
        public async Task<ReturnModelDTO<List<AccomodationTypeDTO>>> GetAccomodationTypes(Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                var result = await _parametricService.GetAccomodationTypesActives();
                if (result.HasError)
                {
                    return CreateErrorResult<List<AccomodationTypeDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<AccomodationTypeDTO>>
                {
                    Data = _mapper.Map<List<AccomodationTypeDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<AccomodationTypeDTO>>("AppParametricService.GetAccomodationTypes", ex, null, useTechnicalMessages);
            }
        }


        private ReturnModelDTO<T> CreateErrorResult<T>(string statusCode, string errorMessage, bool useTechnicalMessages)
        {
            return new ReturnModelDTO<T>
            {
                HasError = true,
                StatusCode = statusCode,
                ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetError(statusCode, errorMessage, useTechnicalMessages))
            };
        }

        private ReturnModelDTO<T> CreateErrorResult<T>(string statusCode, ErrorCommon error, bool useTechnicalMessages)
        {
            return new ReturnModelDTO<T>
            {
                HasError = true,
                StatusCode = statusCode,
                ResultError = _mapper.Map<ErrorCommonDTO>(error)
            };
        }

        private ReturnModelDTO<T> HandleException<T>(string action, Exception ex, object data, bool useTechnicalMessages)
        {
            var error = _errorHandler.GetErrorException(action, ex, data, useTechnicalMessages);
            return new ReturnModelDTO<T>
            {
                HasError = true,
                StatusCode = ResultStatusCode.InternalError,
                ResultError = _mapper.Map<ErrorCommonDTO>(error)
            };
        }
    }
}
