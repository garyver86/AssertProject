using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.Utils;
using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using System.Net;

namespace Assert.Domain.Implementation
{
    public class ListingRent_StepViewService : IListingRent_StepViewService
    {
        private readonly IErrorHandler _errorHandler;
        private readonly IListingRentRepository _listingRentRepository;
        private readonly IPropertySubTypeRepository _propertySubtypeRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IAccommodationTypeRepository _accommodationTypeRepository;
        private readonly IPropertyAddressRepository _propertyAddressRepository;
        private readonly IListingDiscountRepository _listingDiscountRepository;
        private readonly IListingPricingRepository _listingPriceRepository;
        private readonly IListingAmenitiesRepository _listingAmenitiesRepository;
        private readonly IListingFeaturedAspectRepository _listingFeaturedAspectRepository;
        private readonly IListingPhotoRepository _listingPhotoRepository;
        private readonly IListingSecurityItemsRepository _listingSecurityItemsRepository;
        private readonly IViewTypeRepository _viewTypeRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private readonly IFeaturesAspectsRepository _featuresAspectsRepository;
        private readonly IDiscountTypeRepository _discountTypeRepository;
        private readonly IListingRentRulesRepository _listingRentRulesRepository;
        private readonly ISecurityItemsRepository _securityItemsRepository;
        private readonly IApprovalPolityTypeRepository _approvalPolicyTypeRepository;
        private readonly IRulesTypeRepository _rulesTypeRepository;
        private readonly ICancelationPoliciesTypesRepository _cancelationPoliciesTypesRepository;
        public ListingRent_StepViewService(IErrorHandler errorHandler, IPropertySubTypeRepository propertySubtypeRepository,
            IListingRentRepository listingRentRepository, IPropertyRepository propertyRepository, IAccommodationTypeRepository accommodationTypeRepository,
            IPropertyAddressRepository propertyAddressRepository, IListingAmenitiesRepository listingAmenitiesRepository,
            IListingPhotoRepository listingPhotoRepository, IViewTypeRepository viewTypeRepository, IAmenitiesRepository amenitiesRepository,
            IFeaturesAspectsRepository featuresAspectsRepository, IDiscountTypeRepository discountTypeRepository,
            IListingFeaturedAspectRepository listingFeaturedAspectRepository, IListingPricingRepository listingPricingRepository,
            IListingDiscountRepository listingDiscountRepository, IListingSecurityItemsRepository listingSecurityItemsRepository,
            IListingRentRulesRepository listingRentRulesRepository, ISecurityItemsRepository securityItemsRepository,
            IApprovalPolityTypeRepository approvalPolicyTypeRepository, IRulesTypeRepository rulesTypeRepository,
            ICancelationPoliciesTypesRepository cancelationPoliciesTypesRepository)
        {
            _errorHandler = errorHandler;
            _propertySubtypeRepository = propertySubtypeRepository;
            _listingRentRepository = listingRentRepository;
            _propertyRepository = propertyRepository;
            _accommodationTypeRepository = accommodationTypeRepository;
            _propertyAddressRepository = propertyAddressRepository;
            _listingAmenitiesRepository = listingAmenitiesRepository;
            _listingPhotoRepository = listingPhotoRepository;
            _viewTypeRepository = viewTypeRepository;
            _amenitiesRepository = amenitiesRepository;
            _featuresAspectsRepository = featuresAspectsRepository;
            _discountTypeRepository = discountTypeRepository;
            _listingFeaturedAspectRepository = listingFeaturedAspectRepository;
            _listingPriceRepository = listingPricingRepository;
            _listingDiscountRepository = listingDiscountRepository;
            _listingSecurityItemsRepository = listingSecurityItemsRepository;
            _listingRentRulesRepository = listingRentRulesRepository;
            _securityItemsRepository = securityItemsRepository;
            _approvalPolicyTypeRepository = approvalPolicyTypeRepository;
            _rulesTypeRepository = rulesTypeRepository;
            _cancelationPoliciesTypesRepository = cancelationPoliciesTypesRepository;
        }
        public async Task<ReturnModel<ListingProcessDataResultModel>> GetNextListingStepViewData(int? nextViewTypeId, TlListingRent? data, bool useTechnicalMessages)
        {
            TlViewType view = await _viewTypeRepository.Get(nextViewTypeId ?? 1);
            ReturnModel<ListingProcessDataResultModel> result = new ReturnModel<ListingProcessDataResultModel>
            {
                Data = new ListingProcessDataResultModel
                {
                    ListingData = new ListingProcessData_ListingData
                    {
                        ListingRentId = data.ListingRentId,
                        nextViewCode = view.Code,
                    },
                    Parametrics = new ListingProcessData_Parametrics()
                },
                StatusCode = ResultStatusCode.OK,
                HasError = false
            };
            result.Data.ListingData.PropertySubTypeId = data.TpProperties.FirstOrDefault()?.PropertySubtypeId;
            result.Data.ListingData.AccomodationTypeId = data.AccomodationTypeId;
            result.Data.ListingData.MaxGuests = data.MaxGuests;
            result.Data.ListingData.Bathrooms = data.Bathrooms;
            result.Data.ListingData.Bedrooms = data.Bedrooms;
            result.Data.ListingData.Beds = data.Beds;
            result.Data.ListingData.privateBathroom = data.PrivateBathroom;
            result.Data.ListingData.privateBathroomLodging = data.PrivateBathroomLodging;
            result.Data.ListingData.sharedBathroom = data.SharedBathroom;
            var property = data.TpProperties.FirstOrDefault();
            result.Data.ListingData.Latitude = data.TpProperties.FirstOrDefault()?.Latitude;
            result.Data.ListingData.Longitude = data.TpProperties.FirstOrDefault()?.Longitude;
            if (property != null)
            {
                result.Data.ListingData.Address = new TpPropertyAddress
                {
                    Address1 = property?.Address1,
                    Address2 = property?.Address2,
                    CityId = property?.CityId,
                    CountyId = property?.CountyId,
                    City = new TCity
                    {
                        CountyId = property?.CountyId ?? 0,
                        CityId = property?.CityId ?? 0,
                        Name = property?.CityName,
                        County = new TCounty
                        {
                            CountyId = property.CountyId ?? 0,
                            Name = property.CountyName,
                            StateId = property.StateId,
                            State = new TState
                            {
                                StateId = property.StateId ?? 0,
                                Name = property.StateName,
                                CountryId = property.CountryId ?? 0,
                                Country = new TCountry
                                {
                                    CountryId = property.CountryId ?? 0,
                                    Name = property.CountryName
                                }
                            }
                        }
                    }
                };
            }
            result.Data.ListingData.Amenities = await _listingAmenitiesRepository.GetByListingRentId(data.ListingRentId);
            result.Data.ListingData.FeaturedAspects = await _listingFeaturedAspectRepository.GetByListingRentId(data.ListingRentId);
            result.Data.ListingData.SecurityItems = await _listingSecurityItemsRepository.GetByListingRentId(data.ListingRentId);
            result.Data.ListingData.ListingPhotos = await _listingPhotoRepository.GetByListingRentId(data.ListingRentId);
            result.Data.ListingData.Title = data.Name;
            result.Data.ListingData.Description = data.Description;
            result.Data.ListingData.Discounts = await _listingDiscountRepository.Get(data.ListingRentId); //data.TlListingDiscountForRates;
            result.Data.ListingData.PriceNightly = data.TlListingPrices.FirstOrDefault()?.PriceNightly;
            result.Data.ListingData.CurrencyId = data.TlListingPrices.FirstOrDefault()?.CurrencyId;
            result.Data.ListingData.CurrencyCode = data.TlListingPrices.FirstOrDefault()?.Currency?.Code;
            result.Data.ListingData.WeekendNightlyPrice = data.TlListingPrices.FirstOrDefault()?.WeekendNightlyPrice;
            result.Data.ListingData.ApprovalPolicyTypeId = data.ApprovalPolicyTypeId;
            result.Data.ListingData.MinimunNoticeDays = data.MinimumNotice;
            result.Data.ListingData.PreparationDays = data.PreparationDays;
            result.Data.ListingData.TlCheckInOutPolicy = data.TlCheckInOutPolicies.FirstOrDefault();
            result.Data.ListingData.Rules = await _listingRentRulesRepository.GetByListingRentId(data.ListingRentId);
            result.Data.ListingData.CancelationPolicyTypeId = data.CancelationPolicyTypeId;
            switch (view.Code)
            {
                case "LV001":
                    result.Data.Parametrics.PropertySubTypes = await _propertySubtypeRepository.GetActives();
                    result.Data.Parametrics.AccomodationTypes = await _accommodationTypeRepository.GetActives();
                    return result;
                case "LV002":
                    return result;
                case "LV003":
                    //result.Data.ListingData.Address = data.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault();                    
                    return result;
                case "LV004":
                    result.Data.Parametrics.AmenitiesTypes = await _amenitiesRepository.GetActives();
                    result.Data.Parametrics.FeaturedAspects = await _featuresAspectsRepository.GetActives();
                    result.Data.Parametrics.SecurityItems = await _securityItemsRepository.GetActives();
                    return result;
                case "LV005":
                    return result;
                //case "LV006":
                //    result.Data.ListingData.ListingPhotos = await _listingPhotoRepository.GetByListingRentId(data.ListingRentId);
                //    return result;
                case "LV006":
                    return result;
                case "LV007":
                    result.Data.Parametrics.DiscountTypes = await _discountTypeRepository.GetActives();
                    return result;
                case "LV008":
                    result.Data.Parametrics.ApprovalPolicyType = await _approvalPolicyTypeRepository.GetActives();
                    return result;
                case "LV009":
                    result.Data.Parametrics.RuleTypes = await _rulesTypeRepository.GetActives();
                    return result;
                case "LV010":
                    //Devolver información de las políticas de cancelación
                    result.Data.Parametrics.CancelationPolicyTypes = await _cancelationPoliciesTypesRepository.GetActives();
                    return result;
                default:
                    return result;
                    //return new ReturnModel<ListingProcessDataResultModel>
                    //{
                    //    HasError = true,
                    //    StatusCode = ResultStatusCode.BadRequest,
                    //    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "El código de Vista ingresado es inexistente.", useTechnicalMessages)
                    //};
            }
        }

        public async Task<ReturnModel> ProccessListingRentData(TlViewType viewType, TlListingRent listing, int userId, ListingProcessDataModel request_, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            switch (viewType.Code)
            {
                #region Paso 1: Information
                case "LV001":
                    //Vista 1: Definir el tipo de propiedad y el tipo de alojamiento.
                    ReturnModel propertypeResult = await SetPropertyTypeAndAccomodationType(listing, request_, useTechnicalMessages, clientData);
                    return propertypeResult;
                case "LV002":
                    //Vista 2: Definir la capacidad.
                    ReturnModel capacityResult = await SetListingCapacity(listing, request_, useTechnicalMessages, clientData);
                    return capacityResult;
                case "LV003":
                    //Vista 3: Definir la dirección de la propiedad.
                    ReturnModel addressResult = await SetPropertyAddressAndLocation(listing, request_, useTechnicalMessages, clientData);
                    return addressResult;
                #endregion
                #region Paso 2: Description
                case "LV004":
                    //Vista 4: Definir los servicios basicos, destacados y de seguridad.
                    ReturnModel amenitiesResult = await SetServices(listing, request_, useTechnicalMessages, clientData);
                    return amenitiesResult;
                case "LV005":
                    //Vista 5: Validar la cantidad de fotos activas.
                    ReturnModel photosResult = await ValidatePhothos(listing, useTechnicalMessages, clientData);
                    return photosResult;
                //case "LV006":
                //    //Vista 6: Validar la cantidad de fotos activas. (Luego de una eliminación)
                //    ReturnModel photosResult2 = await ValidatePhothos(listing, useTechnicalMessages, clientData);
                //    return photosResult2;
                case "LV006":
                    //Vista 6: Definir el título y la descripción de la propiedad.
                    ReturnModel titleResult = await SetAttibutes(listing, request_, useTechnicalMessages, clientData);
                    return titleResult;
                #endregion
                #region Paso 3: Configuration
                case "LV007":
                    //Vista 7: Definir precios y descuentos.
                    ReturnModel pricingResult = await SetPricingAndDiscount(listing, request_, useTechnicalMessages, clientData);
                    return pricingResult;
                case "LV008":
                    //Vista 8: Definir tipo de reservacion.
                    ReturnModel reservationTypeResult = await SetReservationType(listing, request_, useTechnicalMessages, clientData);
                    return reservationTypeResult;
                case "LV009":
                    //Vista 09: Definir reglas y check-in.
                    ReturnModel rulesResult = await SetCheckInAndRules(listing, request_, useTechnicalMessages, clientData);
                    return rulesResult;
                case "LV010":
                    //Vista 10: Definir las políticas.
                    ReturnModel politicsResult = await SetPolitics(listing, request_, useTechnicalMessages, clientData);
                    return politicsResult;
                case "LV011":
                    //Vista 11: confirmación de la creación
                    ReturnModel reviewConfirmationResult = await SetReviewConfirmation(listing, request_, useTechnicalMessages, clientData);
                    return reviewConfirmationResult;
                #endregion
                default:
                    return new ReturnModel
                    {
                        HasError = true,
                        StatusCode = ResultStatusCode.BadRequest,
                        ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "El código de Vista ingresado es inexistente.", useTechnicalMessages)
                    };
            }
            return null;
        }

        private async Task<ReturnModel> SetPolitics(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            return new ReturnModel
            {
                StatusCode = ResultStatusCode.OK,
                HasError = false
            };
        }

        private async Task<ReturnModel> SetCheckInAndRules(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if ((request_.CheckInPolicies?.CheckInTime.IsNullOrEmpty() ?? true) || (request_.CheckInPolicies?.CheckOutTime.IsNullOrEmpty() ?? true))
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir las horas de Checin y checkout.", useTechnicalMessages)
                };
            }
            await _listingRentRepository.SetCheckInPolicies(listing.ListingRentId, request_.CheckInPolicies?.CheckInTime, request_.CheckInPolicies?.CheckOutTime, request_.CheckInPolicies?.Instructions);
            await _listingRentRulesRepository.Set(listing.ListingRentId, request_.Rules);

            return new ReturnModel
            {
                StatusCode = ResultStatusCode.OK,
                HasError = false
            };
        }

        private async Task<ReturnModel> SetReservationType(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.ApprovalPolicyTypeId == null || (request_.ApprovalPolicyTypeId != 1 && request_.ApprovalPolicyTypeId != 2))
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir el tipo de política de aprobación para la propiedad.", useTechnicalMessages)
                };
            }
            if (request_.MinimunNoticeDays == null || request_.MinimunNoticeDays <= 0)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir el tiempo mínimo de preaviso.", useTechnicalMessages)
                };
            }
            if (request_.PreparationDays == null || request_.PreparationDays <= 0)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir el tiempo de prepareación para que la propiedad esté disponible para el siguiente alquiler.", useTechnicalMessages)
                };
            }
            await _listingRentRepository.SetReservationTypeApprobation(listing.ListingRentId, request_.ApprovalPolicyTypeId.Value, request_.MinimunNoticeDays.Value, request_.PreparationDays.Value);
            return new ReturnModel
            {
                StatusCode = ResultStatusCode.OK,
                HasError = false
            };
        }

        private async Task<ReturnModel> ValidatePhothos(TlListingRent listing, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            var result = await _listingPhotoRepository.GetByListingRentId(listing.ListingRentId);
            if (result.Count < 5)
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe subir al menos 5 fotos de la propiedad.", useTechnicalMessages)
                };
            }
            else
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.OK,
                    HasError = false
                };
            }
        }

        private async Task<ReturnModel> SetPricingAndDiscount(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (!(request_.Pricing > 0))
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir el precio de alquiler por noche  de la propiedad.", useTechnicalMessages)
                };
            }
            if (!(request_.WeekendPrice > 0))
            {
                return new ReturnModel
                {
                    StatusCode = ResultStatusCode.BadRequest,
                    HasError = true,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir el precio de alquiler por noche los fines de semana de la propiedad.", useTechnicalMessages)
                };
            }

            await _listingPriceRepository.SetPricing(listing.ListingRentId, request_.Pricing, request_.WeekendPrice, request_.CurrencyId);
            await _listingDiscountRepository.SetDiscounts(listing.ListingRentId, request_.Discounts?.Select(x => (x.dicountTypeId, x.Price)).ToList());

            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetAttibutes(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.Title.IsNullOrEmpty())
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar un título para la publicación.", useTechnicalMessages)
                };
            }
            if (request_.Title.Length > 32)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"El título de la publicación no debe tener mas de 32 caracteres, usted ingresó {request_.Description.Length}.", useTechnicalMessages)
                };
            }
            if (request_.Description.IsNullOrEmpty())
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar una descripción para la propiedad.", useTechnicalMessages)
                };
            }
            if (request_.Description?.Length > 500)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"La descripción de la propiedad no debe tener mas de 500 caracteres, usted ingresó {request_.Description.Length}.", useTechnicalMessages)
                };
            }
            if (listing.Name != request_.Title || listing.Description != request_.Description)
            {
                await _listingRentRepository.SetNameAndDescription(listing.ListingRentId, request_.Title, request_.Description);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetReviewConfirmation(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.ListingConfirmation ?? false)
            {
                if (listing.ListingRentConfirmationDate == null)
                {
                    await _listingRentRepository.SetAsConfirmed(listing.ListingRentId);
                }
                return new ReturnModel
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            else
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe confirmar la configuración de la propiedad para poder continuar.", useTechnicalMessages)
                };
            }
        }

        private async Task<ReturnModel> SetSecurityConfirmation(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.ExternalCameras == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir si existen camaras de seguridad esteriores.", useTechnicalMessages)
                };
            }
            if (request_.NoiseDesibelesMonitor == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir si la propiedad cuenta con por lo menos un monitor de decibeles.", useTechnicalMessages)
                };
            }
            if (request_.PresenceOfWeapons == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir existe algún tipo de arma dentro de la propiedad.", useTechnicalMessages)
                };
            }
            if (listing.PresenceOfWeapons != request_.PresenceOfWeapons || listing.NoiseDesibelesMonitor != request_.NoiseDesibelesMonitor || listing.ExternalCameras != request_.ExternalCameras)
            {
                await _listingRentRepository.SetSecurityConfirmationData(listing.ListingRentId, listing.PresenceOfWeapons, listing.NoiseDesibelesMonitor, listing.ExternalCameras);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetDiscount(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            await _listingDiscountRepository.SetDiscounts(listing.ListingRentId, request_.Discounts?.Select(x => (x.dicountTypeId, x.Price)).ToList());
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetPricing(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.Pricing > 0)
            {
                var actualPrice = listing.TlListingPrices.FirstOrDefault();
                if (actualPrice?.PriceNightly != request_.Pricing || actualPrice?.CurrencyId != request_.CurrencyId)
                {
                    await _listingPriceRepository.SetPricing(listing.ListingRentId, request_.Pricing, request_.CurrencyId);
                }
                return new ReturnModel
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            else
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir la tarifa po noche de la propiedad.", useTechnicalMessages)
                };
            }
        }

        private async Task<ReturnModel> SetApprovalPolicy(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.ApprovalPolicyTypeId > 0)
            {
                await _listingRentRepository.SetApprovalPolicy(listing.ListingRentId, request_.ApprovalPolicyTypeId);
                return new ReturnModel
                {
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            else
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir seleccionar una política de aprobación para la propiedad.", useTechnicalMessages)
                };
            }
        }

        private async Task<ReturnModel> SetDescription(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.Description.IsNullOrEmpty())
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar una descripción para la propiedad.", useTechnicalMessages)
                };
            }
            if (request_.Description.Length > 500)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"La descripción de la propiedad no debe tener mas de 500 caracteres, usted ingresó {request_.Description.Length}.", useTechnicalMessages)
                };
            }
            if (listing.Description != request_.Description)
            {
                await _listingRentRepository.SetDescription(listing.ListingRentId, request_.Description);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetTitle(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.Title.IsNullOrEmpty())
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar un título para la publicación.", useTechnicalMessages)
                };
            }
            if (request_.Title.Length > 32)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, $"el título de la publicación no debe tener mas de 32 caracteres, usted ingresó {request_.Description.Length}.", useTechnicalMessages)
                };
            }
            if (listing.Name != request_.Title)
            {
                await _listingRentRepository.SetName(listing.ListingRentId, request_.Title);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetServices(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            await _listingAmenitiesRepository.SetListingAmmenities(listing.ListingRentId, request_.FeaturedAmenities, clientData, useTechnicalMessages);
            await _listingFeaturedAspectRepository.SetListingFeaturesAspects(listing.ListingRentId, request_.FeaturedAspects);
            await _listingSecurityItemsRepository.SetListingSecurityItems(listing.ListingRentId, request_.SecurityItems);
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }
        private async Task<ReturnModel> SetAmenities(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            await _listingAmenitiesRepository.SetListingAmmenities(listing.ListingRentId, request_.Amenities, clientData, useTechnicalMessages);
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetListingCapacity(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.Bedrooms < 0 || request_.Bedrooms == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad de habitaciones disponibles en la propiedad.", useTechnicalMessages)
                };
            }
            if (request_.Beds < 0 || request_.Beds == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad de camas disponibles en la propiedad.", useTechnicalMessages)
                };
            }
            if (request_.MaxGuests <= 0 || request_.MaxGuests == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad máxima de huespedes permitidos en la propiedad.", useTechnicalMessages)
                };
            }
            if (request_.Bathrooms < 0 || request_.Bathrooms == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la cantidad de baños que existen en la propiedad.", useTechnicalMessages)
                };
            }
            //if (request_.AllDoorsLooked == null)
            //{
            //    return new ReturnModel
            //    {
            //        HasError = true,
            //        StatusCode = ResultStatusCode.BadRequest,
            //        ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe definir si todas las puertas tienen seguro en la propiedad.", useTechnicalMessages)
            //    };
            //}
            if (listing.Beds != request_.Beds || listing.Bedrooms != request_.Bedrooms || listing.AllDoorsLocked != request_.AllDoorsLooked || listing.MaxGuests != request_.MaxGuests
                || listing.PrivateBathroom != request_.privateBathroom || listing.PrivateBathroomLodging != request_.privateBathroomLodging || listing.SharedBathroom != request_.sharedBathroom)
            {
                await _listingRentRepository.SetCapacity(listing.ListingRentId, request_.Beds, request_.Bedrooms, request_.Bathrooms, request_.MaxGuests, request_.privateBathroom, request_.privateBathroomLodging, request_.sharedBathroom);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetPropertyAddressAndLocation(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (request_.Address == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar la información de la dirección de la propiedad.", useTechnicalMessages)
                };
            }
            TpPropertyAddress addresInput = new TpPropertyAddress
            {
                Address1 = request_.Address.Address1,
                Address2 = request_.Address.Address2,
                ZipCode = request_.Address.ZipCode,
            };

            if (request_.Address.CityId > 0)
            {
                addresInput.CityId = request_.Address.CityId;
                addresInput.CountyId = null;
                addresInput.StateId = null;
            }
            else
            {
                if (request_.Address.CountyId > 0)
                {
                    addresInput.CityId = null;
                    addresInput.CountyId = request_.Address.CountyId;
                    addresInput.StateId = null;
                }
                else
                {
                    if (request_.Address.StateId > 0)
                    {
                        addresInput.CityId = null;
                        addresInput.CountyId = null;
                        addresInput.StateId = request_.Address.StateId;
                    }
                }
            }

            if (request_.Address.Latitude != null && request_.Address.Longitude != null && request_.Address.Latitude != 0 && request_.Address.Longitude != 0)
            {
                if (!GeoUtils.ValidateLatitudLongitude(request_.Address.Latitude, request_.Address.Longitude))
                {
                    return new ReturnModel
                    {
                        HasError = true,
                        StatusCode = ResultStatusCode.BadRequest,
                        ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "La Latitud o Longitud ingresada no es válida, verifica los datos e intenta de nuevo.", useTechnicalMessages)
                    };
                }
            }
            else
            {
                request_.Address.Latitude = null;
                request_.Address.Longitude = null;
            }

            TpPropertyAddress addressResult = await _propertyAddressRepository.Set(addresInput, listing.TpProperties.First().PropertyId);
            if (request_.Address.Latitude != 0 && request_.Address.Longitude != 0)
            {
                Thread.Sleep(1000);
                await _propertyRepository.SetLocation(listing.TpProperties.First().PropertyId, request_.Address.Latitude, request_.Address.Longitude);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetPropertyLocation(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            if (!GeoUtils.ValidateLatitudLongitude(request_.Address.Latitude, request_.Address.Longitude))
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "La Latitud o Longitud ingresada no es válida, verifica los datos e intenta de nuevo.", useTechnicalMessages)
                };
            }
            await _propertyRepository.SetLocation(listing.TpProperties.First().PropertyId, request_.Address.Latitude, request_.Address.Longitude);
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }


        private async Task<ReturnModel> SetAccomodationType(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            TlAccommodationType accomodationType = await _accommodationTypeRepository.GetActive(request_.AccomodationId);
            if (accomodationType == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "Debe ingresar un tipo de alojamiento correcto.", useTechnicalMessages)
                };
            }
            TlListingRent accomodationTypeResult = await _listingRentRepository.SetAccomodationType(listing.ListingRentId, request_.AccomodationId);
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetPropertyTypeAndAccomodationType(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
        {
            TpPropertySubtype propertyType = await _propertySubtypeRepository.GetActive(request_.SubtypeId);
            if (propertyType == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "El tipo de propiedad ingresado es incorrecto.", useTechnicalMessages)
                };
            }
            TlAccommodationType accomodationType = await _accommodationTypeRepository.GetActive(request_.AccomodationId);
            if (accomodationType == null)
            {
                return new ReturnModel
                {
                    HasError = true,
                    StatusCode = ResultStatusCode.BadRequest,
                    ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "El tipo de alojamiento ingresado es incorrecto.", useTechnicalMessages)
                };
            }
            TpProperty setSubtyperesult = await _propertyRepository.SetPropertySubType(listing.TpProperties.First().PropertyId, request_.SubtypeId);
            TlListingRent accomodationTypeResult = await _listingRentRepository.SetAccomodationType(listing.ListingRentId, request_.AccomodationId);

            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }
    }
}
