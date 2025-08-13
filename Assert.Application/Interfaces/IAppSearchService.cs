using Assert.Application.DTOs.Responses;
using Assert.Domain.Models;

namespace Assert.Application.Interfaces
{
    public interface IAppSearchService
    {
        Task<ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>> SearchProperties(SearchFilters filters, int pageNumber, int pageSize, long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<CountryDTO>>> SearchCities(string filter, int filterType, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<LocationSuggestion>>> SuggestLocation(string filter, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<(List<ListingRentDTO> data, PaginationMetadataDTO pagination)>> SearchPropertiesV2(SearchFilters filters, int pageNumber, int pageSize, long userId, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
