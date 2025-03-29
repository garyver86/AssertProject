using Assert.Application.DTOs;
using Assert.Domain.Models;

namespace Assert.Application.Interfaces
{
    public interface IAppSearchService
    {
        Task<ReturnModelDTO<List<ListingRentDTO>>> SearchProperties(SearchFilters filters, Dictionary<string, string> clientData, bool useTechnicalMessages);
        Task<ReturnModelDTO<List<CountryDTO>>> SearchCities(string filter, Dictionary<string, string> clientData, bool useTechnicalMessages);
    }
}
