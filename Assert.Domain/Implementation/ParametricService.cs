using Assert.Domain.Common.Metadata;
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
        private readonly ICancelationPoliciesTypesRepository _cancelationPoliciesTypesRepository;
        private readonly IRulesTypeRepository _rulesTypeRepository;
        private readonly IExceptionLoggerService _exceptionLoggerService;
        private readonly ISecurityItemsRepository _securityItemsRepository;
        private readonly IApprovalPolityTypeRepository _approvalPolicyTypeRepository;
        private readonly IReasonRefuseBookRepository _reasonRefusedBookRepository;
        private readonly IReasonRefusedPriceCalculationRepository _reasonRefusedPriceCalculationRepository;
        private readonly IComplaintReasonRepository _complaintReasonRepository;
        private readonly IBookStatusRepository _bookStatusRepository;
        public ParametricService(IAccommodationTypeRepository accommodationTypeRepository, IErrorHandler errorHandler,
            IFeaturesAspectsRepository featuredAspectsRepository, IDiscountTypeRepository discountTypeRepository,
            IPropertySubTypeRepository propertySubTypeRepository, ISpaceTypeRepository spaceTypeRepository,
            ILanguageRepository languageRepository, IExceptionLoggerService exceptionLoggerService,
            IAmenitiesRepository amenitiesRepository, ICancelationPoliciesTypesRepository cancelationPoliciesTypesRepository,
            IRulesTypeRepository rulesTypeRepository, ISecurityItemsRepository securityItemsRepository,
            IApprovalPolityTypeRepository approvalPolicyTypeRepository, IReasonRefuseBookRepository reasonRefusedBookRepository,
            IReasonRefusedPriceCalculationRepository reasonRefusedPriceCalculationRepository, IBookStatusRepository bookStatusRepository, IComplaintReasonRepository complaintReasonRepository)
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
            _cancelationPoliciesTypesRepository = cancelationPoliciesTypesRepository;
            _rulesTypeRepository = rulesTypeRepository;
            _securityItemsRepository = securityItemsRepository;
            _approvalPolicyTypeRepository = approvalPolicyTypeRepository;
            _reasonRefusedBookRepository = reasonRefusedBookRepository;
            _reasonRefusedPriceCalculationRepository = reasonRefusedPriceCalculationRepository;
            _bookStatusRepository = bookStatusRepository;
            _complaintReasonRepository = complaintReasonRepository;
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

        public async Task<ReturnModel<List<TCancelationPolicyType>>> GetCancellationPolicies(bool useTechnicalMessages)
        {
            ReturnModel<List<TCancelationPolicyType>> result = new ReturnModel<List<TCancelationPolicyType>>();
            try
            {
                var result_data = await _cancelationPoliciesTypesRepository.GetActives();
                return new ReturnModel<List<TCancelationPolicyType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetCancellationPolicies", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TpRuleType>>> GetRentRuleTypes(bool useTechnicalMessages)
        {
            ReturnModel<List<TpRuleType>> result = new ReturnModel<List<TpRuleType>>();
            try
            {
                var result_data = await _rulesTypeRepository.GetActives();
                return new ReturnModel<List<TpRuleType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetRentRuleTypes", ex, null, true);
            }
            return result;
        }
        public async Task<ReturnModel<List<TpSecurityItemType>>> GetSecurityItems(bool useTechnicalMessages)
        {
            ReturnModel<List<TpSecurityItemType>> result = new ReturnModel<List<TpSecurityItemType>>();
            try
            {
                var result_data = await _securityItemsRepository.GetActives();
                return new ReturnModel<List<TpSecurityItemType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetSecurityItems", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TApprovalPolicyType>>> GetApprobalPolicyTypes(bool useTechnicalMessages)
        {
            ReturnModel<List<TApprovalPolicyType>> result = new ReturnModel<List<TApprovalPolicyType>>();
            try
            {
                var result_data = await _approvalPolicyTypeRepository.GetActives();
                return new ReturnModel<List<TApprovalPolicyType>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetApprobalPolicyTypes", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TReasonRefusedBook>>> GetReasonRefusedBook(bool useTechnicalMessages)
        {

            ReturnModel<List<TReasonRefusedBook>> result = new ReturnModel<List<TReasonRefusedBook>>();
            try
            {
                var result_data = await _reasonRefusedBookRepository.GetActives();
                return new ReturnModel<List<TReasonRefusedBook>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetReasonRefusedBook", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TReasonRefusedPriceCalculation>>> GetReasonRefusedPriceCalculation(bool useTechnicalMessages)
        {

            ReturnModel<List<TReasonRefusedPriceCalculation>> result = new ReturnModel<List<TReasonRefusedPriceCalculation>>();
            try
            {
                var result_data = await _reasonRefusedPriceCalculationRepository.GetActives();
                return new ReturnModel<List<TReasonRefusedPriceCalculation>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetReasonRefusedBook", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<TbBookStatus>>> GetBookStatuses(bool useTechnicalMessages)
        {
            ReturnModel<List<TbBookStatus>> result = new ReturnModel<List<TbBookStatus>>();
            try
            {
                var result_data = await _bookStatusRepository.Get();
                return new ReturnModel<List<TbBookStatus>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetBookStatuses", ex, null, true);
            }
            return result;
        }

        public async Task<ReturnModel<List<ComplaintReasonHierarchyDto>>> GetComplaintReasons(bool useTechnicalMessages)
        {
            ReturnModel<List<ComplaintReasonHierarchyDto>> result = new ReturnModel<List<ComplaintReasonHierarchyDto>>();
            try
            {
                //var result_data = await _complaintReasonRepository.GetAll();
                //return new ReturnModel<List<TComplaintReason>>
                //{
                //    StatusCode = ResultStatusCode.OK,
                //    Data = result_data,
                //    HasError = false
                //};
                var result_data = await _complaintReasonRepository.GetComplaintReasonsHierarchyAsync();
                return new ReturnModel<List<ComplaintReasonHierarchyDto>>
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
                result.ResultError = _errorHandler.GetErrorException("IParametricService.GetComplaintReasons", ex, null, true);
            }
            return result;
        }
    }
}
