using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.Utils;
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
        private readonly IViewTypeRepository _viewTypeRepository;
        private readonly IAmenitiesRepository _amenitiesRepository;
        private readonly IFeaturesAspectsRepository _featuresAspectsRepository;
        private readonly IDiscountTypeRepository _discountTypeRepository;
        public ListingRent_StepViewService(IErrorHandler errorHandler, IPropertySubTypeRepository propertySubtypeRepository,
            IListingRentRepository listingRentRepository, IPropertyRepository propertyRepository, IAccommodationTypeRepository accommodationTypeRepository,
            IPropertyAddressRepository propertyAddressRepository, IListingAmenitiesRepository listingAmenitiesRepository,
            IListingPhotoRepository listingPhotoRepository, IViewTypeRepository viewTypeRepository, IAmenitiesRepository amenitiesRepository,
            IFeaturesAspectsRepository featuresAspectsRepository, IDiscountTypeRepository discountTypeRepository,
            IListingFeaturedAspectRepository listingFeaturedAspectRepository, IListingPricingRepository listingPricingRepository,
            IListingDiscountRepository listingDiscountRepository)
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
        }
        public async Task<ReturnModel<ListingProcessDataResultModel>> GetNextListingStepViewData(int? nextViewTypeId, TlListingRent? data, bool useTechnicalMessages)
        {
            TlViewType view = await _viewTypeRepository.Get(nextViewTypeId ?? 0);
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
            switch (view.Code)
            {
                case "LV001":
                    result.Data.Parametrics.PropertySubTypes = await _propertySubtypeRepository.GetActives();
                    result.Data.ListingData.PropertySubTypeId = data.TpProperties.FirstOrDefault()?.PropertySubtypeId;
                    return result;
                case "LV002":
                    result.Data.Parametrics.AccomodationTypes = await _accommodationTypeRepository.GetActives();
                    result.Data.ListingData.AccomodationTypeId = data.AccomodationTypeId;
                    return result;
                case "LV003":
                    result.Data.ListingData.Address = data.TpProperties.FirstOrDefault()?.TpPropertyAddresses.FirstOrDefault();
                    return result;
                case "LV004":
                    result.Data.ListingData.Latitude = data.TpProperties.FirstOrDefault()?.Latitude;
                    result.Data.ListingData.Longitude = data.TpProperties.FirstOrDefault()?.Longitude;
                    return result;
                case "LV005":
                    result.Data.ListingData.MaxGuests = data.MaxGuests;
                    result.Data.ListingData.Bathrooms = data.Bathrooms;
                    result.Data.ListingData.Bedrooms = data.Bedrooms;
                    result.Data.ListingData.Beds = data.Beds;
                    return result;
                case "LV006":
                    result.Data.Parametrics.AmenitiesTypes = await _amenitiesRepository.GetActives();
                    result.Data.ListingData.Amenities = await _listingAmenitiesRepository.GetByListingRentId(data.ListingRentId);
                    return result;
                case "LV007":
                    result.Data.Parametrics.FeaturedAspects = await _featuresAspectsRepository.GetActives();
                    result.Data.ListingData.Title = data.Name;
                    result.Data.ListingData.Description = data.Description;
                    result.Data.ListingData.FeaturedAspects = data.TlListingFeaturedAspects;
                    return result;
                case "LV008":
                    result.Data.Parametrics.DiscountTypes = await _discountTypeRepository.GetActives();
                    //result.Data.ListingData.Discounts = data.Dis.TlListingDiscountForRates;
                    result.Data.ListingData.PriceNightly = data.TlListingPrices.FirstOrDefault()?.PriceNightly;
                    result.Data.ListingData.CurrencyId = data.TlListingPrices.FirstOrDefault()?.CurrencyId;
                    result.Data.ListingData.WeekendNightlyPrice = data.TlListingPrices.FirstOrDefault()?.WeekendNightlyPrice;
                    return result;
                //case "LV009":
                //    result.Data.ListingData.ListingPhotos = await _listingPhotoRepository.GetByListingRentId(data.ListingRentId);
                //    return result;
                //case "LV010":
                //    return result;
                default:
                    return new ReturnModel<ListingProcessDataResultModel>
                    {
                        HasError = true,
                        StatusCode = ResultStatusCode.BadRequest,
                        ResultError = _errorHandler.GetError(ResultStatusCode.BadRequest, "El código de Vista ingresado es inexistente.", useTechnicalMessages)
                    };
            }
        }

        public async Task<ReturnModel> ProccessListingRentData(TlViewType viewType, TlListingRent listing, int userId, ListingProcessDataModel request_, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            switch (viewType.Code)
            {
                #region Paso 1
                case "LV001":
                    ReturnModel propertypeResult = await SetPropertyType(listing, request_, useTechnicalMessages, clientData);
                    return propertypeResult;
                case "LV002":
                    ReturnModel accomodationResult = await SetAccomodationType(listing, request_, useTechnicalMessages, clientData);
                    return accomodationResult;
                case "LV003":
                    ReturnModel addressResult = await SetPropertyAddress(listing, request_, useTechnicalMessages, clientData);
                    return addressResult;
                case "LV004":
                    ReturnModel locationResult = await SetPropertyLocation(listing, request_, useTechnicalMessages, clientData);
                    return locationResult;
                case "LV005":
                    ReturnModel capacityResult = await SetListingCapacity(listing, request_, useTechnicalMessages, clientData);
                    return capacityResult;
                #endregion
                #region Paso 2
                case "LV006":
                    ReturnModel amenitiesResult = await SetAmenities(listing, request_, useTechnicalMessages, clientData);
                    return amenitiesResult;
                case "LV007":
                    ReturnModel titleResult = await SetAttibutes(listing, request_, useTechnicalMessages, clientData);
                    return titleResult;
                #endregion
                #region Paso 3
                case "LV008":
                    ReturnModel pricingResult = await SetPricingAndDiscount(listing, request_, useTechnicalMessages, clientData);
                    //return pricingResult;
                    if (pricingResult.StatusCode == ResultStatusCode.OK)
                    {
                        request_.ListingConfirmation = true;
                        request_.ReviewConfirmatin = true;
                        ReturnModel reviewConfirmationResult = await SetReviewConfirmation(listing, request_, useTechnicalMessages, clientData);
                        return reviewConfirmationResult;
                    }
                    return pricingResult;
                #endregion
                //case "LV009":
                //    ReturnModel photosResult = await SetPhotos(listing, request_, useTechnicalMessages, clientData);
                //    return photosResult;
                //case "LV010":
                //    ReturnModel reviewConfirmationResult = await SetReviewConfirmation(listing, request_, useTechnicalMessages, clientData);
                //    return reviewConfirmationResult;
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
            await _listingDiscountRepository.SetDiscounts(listing.ListingRentId, request_.Discounts?.Select(x => (x.dicountTypeId,x.Price)).ToList());

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
            if (request_.Description.Length > 500)
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
            await _listingFeaturedAspectRepository.SetListingFeaturesAspects(listing.ListingRentId, request_.FeaturedAspects); ;

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
            if (listing.Beds != request_.Beds || listing.Bedrooms != request_.Bedrooms || listing.AllDoorsLocked != request_.AllDoorsLooked || listing.MaxGuests != request_.MaxGuests)
            {
                await _listingRentRepository.SetCapacity(listing.ListingRentId, request_.Beds, request_.Bedrooms, request_.Bathrooms, request_.MaxGuests);
            }
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }

        private async Task<ReturnModel> SetPropertyAddress(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
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
                CityId = request_.Address.CityId,
                ZipCode = request_.Address.ZipCode,
                StateId = request_.Address.StateId,
                CountyId = request_.Address.CountyId
            };
            TpPropertyAddress addressResult = await _propertyAddressRepository.Set(addresInput, listing.TpProperties.First().PropertyId);
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

        private async Task<ReturnModel> SetPropertyType(TlListingRent listing, ListingProcessDataModel request_, bool useTechnicalMessages, Dictionary<string, string> clientData)
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
            TpProperty setSubtyperesult = await _propertyRepository.SetPropertySubType(listing.TpProperties.First().PropertyId, request_.SubtypeId);
            return new ReturnModel
            {
                HasError = false,
                StatusCode = ResultStatusCode.OK
            };
        }
    }
}
