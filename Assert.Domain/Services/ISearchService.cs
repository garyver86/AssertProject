using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Services
{
    public interface ISearchService
    {
        Task<ReturnModel<List<TlListingRent>>> SearchPropertiesAsync(SearchFilters filters);
        Task<ReturnModel<List<TCity>>> SearchCities(string filter, int filterType);
    }
}
