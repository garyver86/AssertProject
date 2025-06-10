using Assert.Domain.Entities;
using Assert.Domain.Exceptions;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Shared.Extensions;

namespace Assert.Domain.Implementation
{
    public class ParametricService : IParametricService
    {
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IDiscountTypeRepository _discountTypeRepository;
        private readonly IFeaturesAspectsRepository _featuredAspectsRepository;
        private readonly IPropertySubTypeRepository _propertySubTypeRepository;
        private readonly ISpaceTypeRepository _spaceTypeRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private readonly IErrorHandler _errorHandler;
        private readonly ILanguageRepository _languageRepository;
        IExceptionLoggerService _exceptionLoggerService;
        public ParametricService(IAccommodationTypeRepository accommodationTypeRepository, IErrorHandler errorHandler,
            IFeaturesAspectsRepository featuredAspectsRepository, IDiscountTypeRepository discountTypeRepository,
            IPropertySubTypeRepository propertySubTypeRepository, ISpaceTypeRepository spaceTypeRepository, 
            ILanguageRepository languageRepository, IExceptionLoggerService exceptionLoggerService, 
            IAmenitiesRepository amenitiesRepository)
        {
            _accommodationTypeRepository = accommodationTypeRepository;
            _errorHandler = errorHandler;
            _featuredAspectsRepository = featuredAspectsRepository;
            _discountTypeRepository = discountTypeRepository;
            _propertySubTypeRepository = propertySubTypeRepository;
            _spaceTypeRepository = spaceTypeRepository;
            _amenitiesRepository = amenitiesRepository;
            _languageRepository = languageRepository;
            _exceptionLoggerService = exceptionLoggerService;
        }

        public async Task<ReturnModel<List<TlAccommodationType>>> GetAccomodationTypesActives()
        {
            ReturnModel<List<TlAccommodationType>> result = new ReturnModel<List<TlAccommodationType>>();
            try
            {
                var result_data = await _accommodationTypeRepository.GetActives();
                return new ReturnModel<List<TlAccommodationType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetAccomodationTypesActives", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TpAmenitiesType>>> GetAmenityTypesActives()
        {
            ReturnModel<List<TpAmenitiesType>> result = new ReturnModel<List<TpAmenitiesType>>();
            try
            {
                var result_data = await _amenitiesRepository.GetActives();
                return new ReturnModel<List<TpAmenitiesType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetAmenityTypesActives", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TDiscountTypeForTypePrice>>> GetDiscountTypes()
        {
            ReturnModel<List<TDiscountTypeForTypePrice>> result = new ReturnModel<List<TDiscountTypeForTypePrice>>();
            try
            {
                var result_data = await _discountTypeRepository.GetActives();
                return new ReturnModel<List<TDiscountTypeForTypePrice>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetDiscountTypes", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TFeaturedAspectType>>> GetFeaturedAspects()
        {
            ReturnModel<List<TFeaturedAspectType>> result = new ReturnModel<List<TFeaturedAspectType>>();
            try
            {
                var result_data = await _featuredAspectsRepository.GetActives();
                return new ReturnModel<List<TFeaturedAspectType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetFeaturedAspects", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TpPropertySubtype>>> GetPropertySubTypes(bool onlyActives)
        {
            ReturnModel<List<TpPropertySubtype>> result = new ReturnModel<List<TpPropertySubtype>>();
            try
            {
                List<TpPropertySubtype> result_data = null;
                if (onlyActives)
                {
                    result_data = await _propertySubTypeRepository.GetActives();
                }
                else
                {
                    result_data = await _propertySubTypeRepository.GetAll();
                }
                return new ReturnModel<List<TpPropertySubtype>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetAccomodationTypesActives", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TSpaceType>>> GetSpaceTypes()
        {
            ReturnModel<List<TSpaceType>> result = new ReturnModel<List<TSpaceType>>();
            try
            {
                var result_data = await _spaceTypeRepository.Get();
                return new ReturnModel<List<TSpaceType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetSpaceTypes", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TLanguage>>> GetLanguages()
        {
            try
            {
                var result_data = await _languageRepository.GetAsync();
                return new ReturnModel<List<TLanguage>>
                {
                    StatusCode = ResultStatusCode.OK,
                    Data = result_data,
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                var (className, methodName) = this.GetCallerInfo();
                _exceptionLoggerService.LogAsync(ex, methodName, className, "Language");
                throw new DomainException(ex.Message);
            }
        }
    }
}
