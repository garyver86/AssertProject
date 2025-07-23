using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Assert.Shared.Extensions;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppParametricService(IParametricService _parametricService,
        IMapper _mapper, IErrorHandler _errorHandler,
        IExceptionLoggerService _exceptionLoggerService) 
        : IAppParametricService
    {
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

        public async Task<ReturnModelDTO<List<AmenityDTO>>> GetAmenityTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TpAmenitiesType>> result = await _parametricService.GetAmenityTypesActives();
                if (result.HasError)
                {
                    return CreateErrorResult<List<AmenityDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<AmenityDTO>>
                {
                    Data = _mapper.Map<List<AmenityDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<AmenityDTO>>("AppParametricService.GetFeaturedAspects", ex, null, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<DiscountDTO>>> GetDiscountTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TDiscountTypeForTypePrice>> result = await _parametricService.GetDiscountTypes();
                if (result.HasError)
                {
                    return CreateErrorResult<List<DiscountDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<DiscountDTO>>
                {
                    Data = _mapper.Map<List<DiscountDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<DiscountDTO>>("AppParametricService.GetDiscountTypes", ex, null, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<FeaturedAspectDTO>>> GetFeaturedAspects(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TFeaturedAspectType>> result = await _parametricService.GetFeaturedAspects();
                if (result.HasError)
                {
                    return CreateErrorResult<List<FeaturedAspectDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<FeaturedAspectDTO>>
                {
                    Data = _mapper.Map<List<FeaturedAspectDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<FeaturedAspectDTO>>("AppParametricService.GetFeaturedAspects", ex, null, useTechnicalMessages);
            }
        }
        public async Task<ReturnModelDTO<List<PropertyTypeDTO>>> GetPropertyTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TpPropertySubtype>> result = await _parametricService.GetPropertySubTypes(true);
                if (result.HasError)
                {
                    return CreateErrorResult<List<PropertyTypeDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<PropertyTypeDTO>>
                {
                    Data = _mapper.Map<List<PropertyTypeDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<PropertyTypeDTO>>("AppParametricService.GetPropertyTypes", ex, null, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<SpaceTypeDTO>>> GetSpaceTypes(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TSpaceType>> result = await _parametricService.GetSpaceTypes();
                if (result.HasError)
                {
                    return CreateErrorResult<List<SpaceTypeDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<SpaceTypeDTO>>
                {
                    Data = _mapper.Map<List<SpaceTypeDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<SpaceTypeDTO>>("AppParametricService.GetSpaceTypes", ex, null, useTechnicalMessages);
            }
        }
        
        public async Task<ReturnModelDTO<List<LanguageDTO>>> GetLanguageTypes()
        {
            try
            {
                var languages = await _parametricService.GetLanguages();
                
                return new ReturnModelDTO<List<LanguageDTO>>
                {
                    Data = _mapper.Map<List<LanguageDTO>>(languages.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, "Languages");

                throw new ApplicationException(ex.Message);
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

        public async Task<ReturnModelDTO<List<CancellationPolicyDTO>>> GetCancellationPolicies(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TCancelationPolicyType>> result = await _parametricService.GetCancellationPolicies(useTechnicalMessages);
                if (result.HasError)
                {
                    return CreateErrorResult<List<CancellationPolicyDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<CancellationPolicyDTO>>
                {
                    Data = _mapper.Map<List<CancellationPolicyDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<CancellationPolicyDTO>>("AppParametricService.GetCancellationPolicies", ex, null, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<RentRuleDTO>>> GetRentRules(Dictionary<string, string> requestInfo, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<List<TpRuleType>> result = await _parametricService.GetRentRuleTypes(useTechnicalMessages);
                if (result.HasError)
                {
                    return CreateErrorResult<List<RentRuleDTO>>(result.StatusCode, result.ResultError, useTechnicalMessages);
                }
                return new ReturnModelDTO<List<RentRuleDTO>>
                {
                    Data = _mapper.Map<List<RentRuleDTO>>(result.Data),
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<RentRuleDTO>>("AppParametricService.GetCancellationPolicies", ex, null, useTechnicalMessages);
            }
        }
    }
}
