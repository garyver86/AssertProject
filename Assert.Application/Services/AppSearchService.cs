using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Assert.Domain.ValueObjects;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Assert.Application.Services
{
    public class AppSearchService : IAppSearchService
    {
        private readonly ISearchService _searchService;
        private readonly ILocationSugestionService _locationService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;
        private readonly IHttpContextAccessor requestContext;
        public AppSearchService(ISearchService searchService, IMapper mapper, IErrorHandler errorHandler, 
            ILocationSugestionService locationService, ISystemConfigurationRepository systemConfigurationRepository,
            IHttpContextAccessor contextAccessor)
        {
            _searchService = searchService;
            _mapper = mapper;
            _errorHandler = errorHandler;
            _locationService = locationService;
            _systemConfigurationRepository = systemConfigurationRepository;
            requestContext = contextAccessor;
        }

        public async Task<ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>> SearchProperties(SearchFilters filters, int pageNumber, int pageSize, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                ReturnModel<(List<TlListingRent> data, PaginationMetadata pagination)> listings = await _searchService.SearchPropertiesAsync(filters, pageNumber, pageSize);
                //var dataResult = _mapper.Map<List<ListingRentDTO>>(listings.Data);

                //return new ReturnModelDTO<List<ListingRentDTO>>
                //{
                //    Data = dataResult,
                //    HasError = false,
                //    StatusCode = ResultStatusCode.OK
                //};

                if (listings.Data.data?.Count > 0)
                {
                    string _basePath = await _systemConfigurationRepository.GetListingResourcePath();
                    _basePath = _basePath.Replace("\\", "/").Replace("wwwroot/Assert/", "");
                    foreach (var list in listings.Data.data)
                    {
                        if (list?.TlListingPhotos?.Count > 0)
                        {
                            foreach (var item in list.TlListingPhotos)
                            {
                                item.PhotoLink = $"{requestContext.HttpContext?.Request.Scheme}://{requestContext.HttpContext?.Request.Host}/{_basePath}/{item.Name}";
                            }
                        }
                    }
                }

                return new ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>
                {
                    HasError = listings.HasError,
                    StatusCode = listings.StatusCode,
                    ResultError = _mapper.Map<ErrorCommonDTO>(listings.ResultError),
                    Data = (_mapper.Map<List<ListingRentDTO>>(listings.Data.data), _mapper.Map<PaginationMetadataDTO>(listings.Data.pagination))
                };
            }
            catch (Exception ex)
            {
                return HandleException<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>("AppSearchProperties.SearchProperties", ex, new { filters }, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<CountryDTO>>> SearchCities(string filter, int filterType, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            if (string.IsNullOrEmpty(filter) || filter.Length < 3)
            {
                return CreateErrorResult<List<CountryDTO>>(ResultStatusCode.BadRequest, "El filtro de búsqueda debe tener al menos 3 caracteres.", useTechnicalMessages);
            }

            try
            {
                var citiesResult = await _searchService.SearchCities(filter, filterType);
                if (citiesResult.HasError)
                {
                    return CreateErrorResult<List<CountryDTO>>(citiesResult.StatusCode, citiesResult.ResultError, useTechnicalMessages);
                }

                var groupedCountries = GroupCitiesByCountry(citiesResult.Data);
                return new ReturnModelDTO<List<CountryDTO>>
                {
                    Data = groupedCountries,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<CountryDTO>>("AppSearchProperties.SearchCities", ex, new { filter }, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<LocationSuggestion>>> SuggestLocation(string filter, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            if (string.IsNullOrEmpty(filter) || filter.Length < 3)
            {
                return CreateErrorResult<List<LocationSuggestion>>(ResultStatusCode.BadRequest, "El filtro de búsqueda debe tener al menos 3 caracteres.", useTechnicalMessages);
            }

            try
            {
                var citiesResult = await _locationService.GetLocationSuggestions(filter);
                if (citiesResult.HasError)
                {
                    return CreateErrorResult<List<LocationSuggestion>>(citiesResult.StatusCode, citiesResult.ResultError, useTechnicalMessages);
                }

                return new ReturnModelDTO<List<LocationSuggestion>>
                {
                    Data = citiesResult.Data,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<LocationSuggestion>>("AppSearchProperties.SuggestLocation", ex, new { filter }, useTechnicalMessages);
            }
        }

        private List<CountryDTO> GroupCitiesByCountry(List<TCity> cities)
        {
            return cities
                .GroupBy(city => city.County?.State?.Country)
                .Where(group => group.Key != null && !(group.Key.IsDisabled ?? false))
                .Select(group => new CountryDTO
                {
                    CountryId = group.Key.CountryId,
                    Name = group.Key.Name,
                    States = group
                        .GroupBy(city => city.County?.State)
                        .Where(stateGroup => stateGroup.Key != null && !(stateGroup.Key.IsDisabled ?? false))
                        .Select(stateGroup => new StateDTO
                        {
                            StateId = stateGroup.Key.StateId,
                            Name = stateGroup.Key.Name,
                            Counties = stateGroup
                                .GroupBy(city => city.County)
                                .Where(countyGroup => countyGroup.Key != null && !(countyGroup.Key.IsDisabled ?? false))
                                .Select(countyGroup => new CountyDTO
                                {
                                    CountyId = countyGroup.Key.CountyId,
                                    Name = countyGroup.Key.Name,
                                    Cities = countyGroup
                                        .Select(city => new City2DTO
                                        {
                                            CityId = city.CityId,
                                            Name = city.Name
                                        }).ToList()
                                }).ToList()
                        }).ToList()
                }).ToList();
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
