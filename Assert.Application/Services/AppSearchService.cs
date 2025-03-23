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
