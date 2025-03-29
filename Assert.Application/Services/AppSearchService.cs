using Assert.Application.DTOs;
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

        public async Task<ReturnModelDTO<List<CountryDTO>>> SearchCities(string filter, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<List<CountryDTO>> result = new ReturnModelDTO<List<CountryDTO>>();
            try
            {
                if (string.IsNullOrEmpty(filter) || filter.Length < 3)
                {
                    return new ReturnModelDTO<List<CountryDTO>>
                    {
                        HasError = true,
                        StatusCode = ResultStatusCode.BadRequest,
                        ResultError = _mapper.Map<ErrorCommonDTO>(_errorHandler.GetError(ResultStatusCode.BadRequest, "El filtro de búsqueda debe tener al menos 3 caracteres.", useTechnicalMessages))
                    };
                }
                ReturnModel<List<TCity>> citiesResult = await _searchService.SearchCities(filter);
                if (citiesResult.HasError)
                {
                    var dataResult = _mapper.Map<List<CountryDTO>>(citiesResult.Data);
                    result = new ReturnModelDTO<List<CountryDTO>>
                    {
                        HasError = true,
                        StatusCode = citiesResult.StatusCode,
                        ResultError = _mapper.Map<ErrorCommonDTO>(citiesResult.ResultError),
                    };
                }
                else
                {
                    var cities = citiesResult.Data;
                    var paisesAgrupados = cities
                       .GroupBy(ciudad => ciudad.County?.State?.Country)
                       .Where(grupoPais => grupoPais.Key != null && !(grupoPais.Key.IsDisabled ?? false))
                       .Select(grupoPais => new CountryDTO
                       {
                           CountryId = grupoPais.Key.CountryId,
                           Name = grupoPais.Key.Name,
                           States = grupoPais
                               .GroupBy(ciudad => ciudad.County?.State)
                               .Where(grupoEstado => grupoEstado.Key != null && !(grupoEstado.Key.IsDisabled ?? false))
                               .Select(grupoEstado => new StateDTO
                               {
                                   StateId = grupoEstado.Key.StateId,
                                   Name = grupoEstado.Key.Name,
                                   Counties = grupoEstado
                                       .GroupBy(ciudad => ciudad.County)
                                       .Where(grupoCondado => grupoCondado.Key != null && !(grupoCondado.Key.IsDisabled ?? false))
                                       .Select(grupoCondado => new CountyDTO
                                       {
                                           CountyId = grupoCondado.Key.CountyId,
                                           Name = grupoCondado.Key.Name,
                                           Cities = grupoCondado
                                               .Select(ciudad => new City2DTO
                                               {
                                                   CityId = ciudad.CityId,
                                                   Name = ciudad.Name
                                               }).ToList()
                                       }).ToList()
                               }).ToList()
                       }).ToList();

                    return new ReturnModelDTO<List<CountryDTO>>
                    {
                        Data = paisesAgrupados,
                        HasError = false,
                        StatusCode = ResultStatusCode.OK
                    };
                }

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                var error = _errorHandler.GetErrorException("AppSearchProperties.SearchCities", ex, new { filter }, useTechnicalMessages);
                result.ResultError = _mapper.Map<ErrorCommonDTO>(error);
            }
            return result;


        }

        public async Task<ReturnModelDTO<List<ListingRentDTO>>> SearchProperties(SearchFilters filters, Dictionary<string, string> clientData, bool useTechnicalMessages)
        {
            ReturnModelDTO<List<ListingRentDTO>> result = new ReturnModelDTO<List<ListingRentDTO>>();
            try
            {
                ReturnModel<List<TlListingRent>> listings = await _searchService.SearchPropertiesAsync(filters);

                var dataResult = _mapper.Map<List<ListingRentDTO>>(listings.Data);
                result = new ReturnModelDTO<List<ListingRentDTO>>
                {
                    Data = dataResult,
                    HasError = false,
                    StatusCode = ResultStatusCode.OK
                };

            }
            catch (Exception ex)
            {
                result.StatusCode = ResultStatusCode.InternalError;
                result.HasError = true;
                var error = _errorHandler.GetErrorException("AppSearchProperties.SearchProperties", ex, new { filters }, useTechnicalMessages);
                result.ResultError = _mapper.Map<ErrorCommonDTO>(error);
            }
            return result;
        }
    }
}
