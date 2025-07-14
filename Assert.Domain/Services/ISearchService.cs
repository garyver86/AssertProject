using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.ValueObjects;

namespace Assert.Domain.Services
{
    public interface ISearchService
    {
        Task<ReturnModel<(List<TlListingRent>, PaginationMetadata)>> SearchPropertiesAsync(SearchFilters filters, int pageNumber, int pageSize, long userId);
        Task<ReturnModel<List<TCity>>> SearchCities(string filter, int filterType);
    }
}
