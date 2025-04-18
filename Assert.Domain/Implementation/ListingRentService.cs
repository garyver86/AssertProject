﻿using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace Assert.Domain.Implementation
{
    public class ListingRentService : IListingRentService
    {
        private readonly IConfiguration _configuration;

        private readonly IListingRentRepository _listingRentRepository;
        private readonly IListingStatusRepository _listingStatusRepository;
        private readonly IErrorHandler _errorHandler;
        private readonly IUserRepository _userRepository;
        private readonly IPhoneRepository _phoneRepository;
        private readonly IStepsTypeRepository _stepsTypeRepository;
        private readonly IViewTypeRepository _viewTypeRepository;
        private readonly IListingStepViewRepository _listingViewStepRepository;
        private readonly IListingStepRepository _listingStepRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IQuickTipRepository _quickTipRepository;

        private readonly IListingSpaceRepository _listingSpaceRepository;
        private readonly IListingAmenitiesRepository _listingAmenitiesRepository;
        private readonly IListingDiscountForRateRepository _listingDiscountForRateRepository;
        private readonly IListingRentRulesRepository _listingRentRulesRepository;
        private readonly IListingPhotoRepository _listingPhotoRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IListingLogRepository _listingLogRepository;

        private readonly IListingRent_StepViewService _StepViewService;


        public ListingRentService(IConfiguration configuration, IListingRentRepository listingRentRepository, IListingStatusRepository listingStatusRepository,
            IErrorHandler errorHandler, IUserRepository userRepository, IPhoneRepository phoneRepository, IStepsTypeRepository stepsTypeRepository,
            IViewTypeRepository viewTypeRepository, IListingStepViewRepository listingViewStepRepository, IListingStepRepository listingStepRepository,
            IPropertyRepository propertyRepository, IQuickTipRepository quickTipRepository, IListingSpaceRepository listingSpaceRepository, IListingAmenitiesRepository listingAmenitiesRepository,
            IListingDiscountForRateRepository listingDiscountForRateRepository, IListingRentRulesRepository listingRentRulesRepository, IListingPhotoRepository listingPhotoRepository,
            ICityRepository cityRepository, IStateRepository stateRepository, ICountryRepository countryRepository, IListingLogRepository listingLogRepository, IListingRent_StepViewService stepViewService)
        {
            _listingRentRepository = listingRentRepository;
            _listingStatusRepository = listingStatusRepository;
            _errorHandler = errorHandler;
            _userRepository = userRepository;
            _phoneRepository = phoneRepository;
            _stepsTypeRepository = stepsTypeRepository;
            _viewTypeRepository = viewTypeRepository;
            _listingViewStepRepository = listingViewStepRepository;
            _listingStepRepository = listingStepRepository;
            _propertyRepository = propertyRepository;
            _quickTipRepository = quickTipRepository;
            _listingSpaceRepository = listingSpaceRepository;
            _listingAmenitiesRepository = listingAmenitiesRepository;
            _listingDiscountForRateRepository = listingDiscountForRateRepository;
            _listingRentRulesRepository = listingRentRulesRepository;
            _listingPhotoRepository = listingPhotoRepository;
            _cityRepository = cityRepository;
            _configuration = configuration;
            _stateRepository = stateRepository;
            _countryRepository = countryRepository;
            _listingLogRepository = listingLogRepository;
            _StepViewService = stepViewService;
        }
        public async Task<ReturnModel> ChangeStatus(long listingRentId, int ownerUserId, string newStatusCode, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModel result = new ReturnModel();
            try
            {
                TlListingRent listingRent = await _listingRentRepository.Get(listingRentId, ownerUserId);
                if (listingRent != null)
                {
                    TlListingStatus status = await _listingStatusRepository.Get(newStatusCode);
                    if (status != null)
                    {
                        TlListingStatus oldStatus = await _listingStatusRepository.Get(listingRent.ListingStatusId ?? 0);

                        if (listingRent.ListingStatusId == status.ListingStatusId)
                        {
                            result = new ReturnModel
                            {
                                StatusCode = ResultStatusCode.OK,
                                HasError = false
                            };
                        }
                        else
                        {
                            switch (status.ListingStatusId)
                            {
                                case 1:
                                    result = new ReturnModel
                                    {
                                        StatusCode = ResultStatusCode.BadRequest,
                                        HasError = true,
                                        ResultError = _errorHandler.GetError(ConstantsHelp.ERR_1, "Estado " + status.Code + " no soportado para este servicio.", useTechnicalMessages)
                                    };
                                    break;
                                case 2:
                                    if (listingRent.ListingStatusId == 1)
                                    {
                                        var changeResult2 = await _listingRentRepository.ChangeStatus(listingRentId, ownerUserId, status.ListingStatusId, clientData);

                                        result = new ReturnModel
                                        {
                                            HasError = false,
                                            StatusCode = ResultStatusCode.OK
                                        };
                                    }
                                    else
                                    {
                                        result = new ReturnModel
                                        {
                                            StatusCode = ResultStatusCode.BadRequest,
                                            HasError = true,
                                            ResultError = _errorHandler.GetError(ConstantsHelp.ERR_1, "No es posible pasar del estado " + oldStatus.Code + " al estado " + status.Code, useTechnicalMessages)
                                        };
                                    }
                                    break;
                                case 3:
                                    if (listingRent.ListingStatusId == 4)
                                    {
                                        var changeResult2 = await _listingRentRepository.ChangeStatus(listingRentId, ownerUserId, status.ListingStatusId, clientData);

                                        result = new ReturnModel
                                        {
                                            HasError = false,
                                            StatusCode = ResultStatusCode.OK
                                        };
                                    }
                                    else
                                    {
                                        result = new ReturnModel
                                        {
                                            StatusCode = ResultStatusCode.BadRequest,
                                            HasError = true,
                                            ResultError = _errorHandler.GetError(ConstantsHelp.ERR_1, "No es posible pasar del estado " + oldStatus.Code + " al estado " + status.Code, useTechnicalMessages)
                                        };
                                    }
                                    break;
                                case 4:
                                    if (listingRent.ListingStatusId == 3)
                                    {
                                        var changeResult3 = await _listingRentRepository.ChangeStatus(listingRentId, ownerUserId, status.ListingStatusId, clientData);

                                        result = new ReturnModel
                                        {
                                            HasError = false,
                                            StatusCode = ResultStatusCode.OK
                                        };
                                    }
                                    else
                                    {
                                        result = new ReturnModel
                                        {
                                            StatusCode = ResultStatusCode.BadRequest,
                                            HasError = true,
                                            ResultError = _errorHandler.GetError(ConstantsHelp.ERR_1, "No es posible pasar del estado " + oldStatus.Code + " al estado " + status.Code, useTechnicalMessages)
                                        };
                                    }
                                    break;
                                case 5:
                                    var changeResult = await _listingRentRepository.ChangeStatus(listingRentId, ownerUserId, status.ListingStatusId, clientData);
                                    break;
                                default:
                                    result = new ReturnModel
                                    {
                                        StatusCode = ResultStatusCode.BadRequest,
                                        HasError = true,
                                        ResultError = _errorHandler.GetError(ConstantsHelp.ERR_1, "Estado " + status.Code + " no soportado.", useTechnicalMessages)
                                    };
                                    break;
                            }
                        }
                    }
                    else
                    {
                        result = new ReturnModel
                        {
                            StatusCode = ResultStatusCode.BadRequest,
                            HasError = true,
                            ResultError = _errorHandler.GetError(ConstantsHelp.DB_0, "ListingStatus " + newStatusCode, useTechnicalMessages)
                        };
                    }
                }
                else
                {
                    result = new ReturnModel
                    {
                        StatusCode = ResultStatusCode.BadRequest,
                        HasError = true,
                        ResultError = _errorHandler.GetError(ConstantsHelp.DB_0, "TlListingRent " + listingRentId, useTechnicalMessages)
                    };
                }
            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                result.ResultError = _errorHandler.GetErrorException("L_ListingRentView.ChangeStatus", ex, new { listingRentId, ownerUserId, newStatusCode, clientData }, useTechnicalMessages);
            }
            return result;
        }

        public Task<ReturnModel<List<TlListingRent>>> GetAllListingsRentsData(int ownerUserId, bool UseTechnicalMessages)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetCalendar(long listingRentId, int ownerUserId, string startDate, string endDate)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetListingData(int? ownerId, int listingRentId, bool UseTechnicalMessages)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetOwnerCalendar(int ownerUserId, string startDate, string endDate)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPriceNightlySuggestion(int spaces, int year, int amenities, bool UseTechnicalMessages)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetViewsData(int ownerUserId, int listingRentId)
        {
            throw new NotImplementedException();
        }
        public Task<string> SetCalendar(long listingRentId, int ownerUserId, string startDate, string endDate, List<CalendarEvent> calendarEvents)
        {
            throw new NotImplementedException();
        }

        public Task<string> SetCalendarByDay(long listingRentId, int ownerUserId, string startDate, string endDate, List<YearCalendarEvent> Years)
        {
            throw new NotImplementedException();
        }

        public async Task<ReturnModel<ListingProcessDataResultModel>> ProcessData(long? listingRentId, string viewCode, ListingProcessDataModel request_, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            string userId = clientData["UserId"];
            int _userID = -1;
            if (userId.IsNullOrEmpty())
            {
                //Devolver Error de no autorizado
                return new ReturnModel<ListingProcessDataResultModel>
                {
                    StatusCode = ResultStatusCode.Unauthorized,
                    ResultError = _errorHandler.GetError(ResultStatusCode.Unauthorized, "Usuario no autorizado para este recurso.", useTechnicalMessages)
                };
            }
            else
            {
                int.TryParse(userId, out _userID);
                TlViewType viewType = await _viewTypeRepository.GetByCode(viewCode);
                if (viewType == null)
                {
                    return new ReturnModel<ListingProcessDataResultModel>
                    {
                        StatusCode = ResultStatusCode.NotFound,
                        ResultError = _errorHandler.GetError(ResultStatusCode.NotFound, $"El código de vista {viewCode} no generó ningún resultado.", useTechnicalMessages)
                    };
                }
                else
                {
                    //Implementación de la logica detrás del procesamiento de los datos de los listings en base as las vistas definidas
                    if ((listingRentId == null || listingRentId < 0) && viewCode != "LV001")
                    {
                        //si no es error.
                        return new ReturnModel<ListingProcessDataResultModel>
                        {
                            StatusCode = ResultStatusCode.BadRequest,
                            ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"El código de vista {viewCode} necesita un listingId.", useTechnicalMessages)
                        };
                    }
                    else if ((listingRentId == null || listingRentId <= 0) && viewCode == "LV001")
                    {
                        if (request_.Bedrooms < 0 || request_.Bedrooms == null)
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                HasError = true,
                                StatusCode = ResultStatusCode.BadRequest,
                                ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad de habitaciones disponibles en la propiedad.", useTechnicalMessages)
                            };
                        }
                        if (request_.Beds < 0 || request_.Beds == null)
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                HasError = true,
                                StatusCode = ResultStatusCode.BadRequest,
                                ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad de camas disponibles en la propiedad.", useTechnicalMessages)
                            };
                        }
                        if (request_.MaxGuests <= 0 || request_.MaxGuests == null)
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                HasError = true,
                                StatusCode = ResultStatusCode.BadRequest,
                                ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad máxima de huespedes permitidos en la propiedad.", useTechnicalMessages)
                            };
                        }
                        if (request_.Bathrooms < 0 || request_.Bathrooms == null)
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                HasError = true,
                                StatusCode = ResultStatusCode.BadRequest,
                                ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad de baños que existen en la propiedad.", useTechnicalMessages)
                            };
                        }
                        ReturnModel<TlListingRent> newListing = await InitializeListingRent(viewType, request_, _userID, clientData, useTechnicalMessages);
                        if (newListing.StatusCode == ResultStatusCode.OK)
                        {
                            await _listingViewStepRepository.SetEnded(listingRentId ?? 0, viewType.ViewTypeId, true);

                            ReturnModel<ListingProcessDataResultModel> NextStepResult = await _StepViewService.GetNextListingStepViewData(viewType.NextViewTypeId, newListing.Data, useTechnicalMessages);
                            return NextStepResult;
                        }
                        else
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                StatusCode = newListing.StatusCode,
                                HasError = newListing.HasError,
                                ResultError = newListing.ResultError
                            };
                        }
                    }
                    else
                    {
                        //Se procesan las siguientes vistas, en las cuales se necesita el lisging rent. (Si se encuentra publicado no deberia poder modificar.)
                        var listing = await _listingRentRepository.Get(listingRentId ?? 0, _userID);
                        if (listing == null || listing.ListingStatusId == 5)
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                StatusCode = ResultStatusCode.NotFound,
                                ResultError = _errorHandler.GetError(ResultStatusCode.NotFound, $"El listing rent con id {listingRentId} no fué encontrado.", useTechnicalMessages)
                            };
                        }
                        else if (listing.ListingStatusId == 3)
                        {
                            return new ReturnModel<ListingProcessDataResultModel>
                            {
                                StatusCode = ResultStatusCode.BadRequest,
                                ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"El listing rent con id {listingRentId} no puede ser editado porque se encuentra en estado {listing.ListingStatus.Code}.", useTechnicalMessages)
                            };
                        }
                        else
                        {
                            ReturnModel processDataResult = await _StepViewService.ProccessListingRentData(viewType, listing, _userID, request_, clientData, useTechnicalMessages);
                            if (processDataResult.StatusCode == ResultStatusCode.OK)
                            {
                                await _listingViewStepRepository.SetEnded(listingRentId ?? 0, viewType.ViewTypeId, true);

                                if (viewType.NextViewTypeId == null)
                                {
                                    ReturnModel resultStatuses = await _listingViewStepRepository.IsAllViewsEndeds(listingRentId ?? 0);
                                    if (resultStatuses.StatusCode == ResultStatusCode.OK)
                                    {
                                        var newStatus = await ChangeStatus(listingRentId ?? 0, _userID, "COMPLETED", clientData, useTechnicalMessages);
                                        if (newStatus.StatusCode != ResultStatusCode.OK)
                                        {
                                            return new ReturnModel<ListingProcessDataResultModel>
                                            {
                                                HasError = false,
                                                StatusCode = ResultStatusCode.Accepted,
                                                ResultError = newStatus.ResultError
                                            };
                                        }

                                        return new ReturnModel<ListingProcessDataResultModel>
                                        {
                                            HasError = false,
                                            StatusCode = ResultStatusCode.OK
                                        };
                                    }
                                    else
                                    {
                                        return new ReturnModel<ListingProcessDataResultModel>
                                        {
                                            HasError = false,
                                            StatusCode = ResultStatusCode.Accepted,
                                            ResultError = resultStatuses.ResultError
                                        };
                                    }
                                }
                                else
                                {

                                    ReturnModel<ListingProcessDataResultModel> NextStepResult = await _StepViewService.GetNextListingStepViewData(viewType.NextViewTypeId, listing, useTechnicalMessages);
                                    return NextStepResult;
                                }
                            }
                            else
                            {
                                return new ReturnModel<ListingProcessDataResultModel>
                                {
                                    StatusCode = processDataResult.StatusCode,
                                    HasError = processDataResult.HasError,
                                    ResultError = processDataResult.ResultError
                                };
                            }
                        }
                    }
                }
            }
        }

        private async Task<ReturnModel<TlListingRent>> InitializeListingRent(TlViewType viewType, ListingProcessDataModel request_, int userId, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModel<TlListingRent> rstl = null;
            if (viewType.Code == "LV001")
            {

                TlListingRent listingRent = new TlListingRent
                {
                    OwnerUserId = userId,
                    ListingStatusId = 1
                };

                List<TlStepsType> stepsTypes = await _stepsTypeRepository.GetAllActives();
                foreach (var step in stepsTypes)
                {
                    List<TlViewType> views = await _viewTypeRepository.GetByType(step.StepsTypeId);
                    TlListingStep _step = new TlListingStep
                    {
                        StepsTypeId = step.StepsTypeId,
                        TlListingStepsViews = views.Select(x => new TlListingStepsView
                        {
                            ViewTypeId = x.ViewTypeId
                        }).ToList()
                    };
                    listingRent.TlListingSteps.Add(_step);
                }

                TlListingRent result = await _listingRentRepository.Register(listingRent, clientData);

                if (result != null)
                {
                    var property = await _propertyRepository.Register(listingRent.ListingRentId);
                    ReturnModel processResult = await _StepViewService.ProccessListingRentData(viewType, result, userId, request_, clientData, useTechnicalMessages);
                    if (processResult.StatusCode == ResultStatusCode.OK)
                    {
                        return new ReturnModel<TlListingRent>
                        {
                            StatusCode = processResult.StatusCode,
                            Data = result
                        };
                    }
                    else
                    {
                        return new ReturnModel<TlListingRent>
                        {
                            StatusCode = processResult.StatusCode,
                            HasError = processResult.HasError,
                            ResultError = processResult.ResultError
                        };
                    }
                }
                else
                {
                    return new ReturnModel<TlListingRent>
                    {
                        HasError = true,
                        ResultError = _errorHandler.GetError(ResultStatusCode.NotFound, "No fue posible procesar la solicitud.", useTechnicalMessages)
                    };
                }
            }
            else
            {
                rstl = new ReturnModel<TlListingRent>
                {
                    HasError = true,
                    ResultError = _errorHandler.GetError(ConstantsHelp.ERR_1, "Parametros Incorrectos " + viewType.Code, useTechnicalMessages)

                };
            }

            return rstl;
        }
    }
}
