using Assert.Application.DTOs.Responses;
using Assert.Application.Interfaces;
using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Services;
using AutoMapper;

namespace Assert.Application.Services
{
    public class AppSearchService : IAppSearchService
    {
        private readonly ISearchService _searchService;
        private readonly IMapper _mapper;
        private readonly IErrorHandler _errorHandler;
        public AppSearchService(ISearchService searchService, IMapper mapper, IErrorHandler errorHandler)
        {
            _searchService = searchService;
            _mapper = mapper;
            _errorHandler = errorHandler;
        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> SearchProperties(SearchFilters filters, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            try
            {
                var listings = await _searchService.SearchPropertiesAsync(filters);
                var dataResult = _mapper.Map<List<ListingRentDTO>>(listings.Data);

                return new ReturnModelDTO<List<ListingRentDTO>>
                {
                    Data = dataResult,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return HandleException<List<ListingRentDTO>>("AppSearchProperties.SearchProperties", ex, new { filters }, useTechnicalMessages);
            }
        }

        public async Task<ReturnModelDTO<List<CountryDTO>>> SearchCities(string filter, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            if (string.IsNullOrEmpty(filter) || filter.Length < 3)
            {
                return CreateErrorResult<List<CountryDTO>>(ResultStatusCode.BadRequest, "El filtro de búsqueda debe tener al menos 3 caracteres.", useTechnicalMessages);
            }

            try
            {
                var citiesResult = await _searchService.SearchCities(filter);
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
