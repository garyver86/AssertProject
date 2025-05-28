using Assert.Application.DTOs.Responses;
using Assert.Domain.Models;

namespace Assert.Application.Interfaces
{
    public interface IAppSearchService
    {
        Task<ReturnModelDTO<List<ListingRentDTO>>> SearchProperties(SearchFilters filters, int pageNumber, int pageSize, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<CountryDTO>>> SearchCities(string filter, int filterType, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<LocationSuggestion>>> SuggestLocation(string filter, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
