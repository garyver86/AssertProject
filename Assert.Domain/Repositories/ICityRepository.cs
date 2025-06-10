using Assert.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Assert.Domain.Repositories
{
    public interface ICityRepository
    {
        TCity GetById(int cityId);
        Task<TCity> GetByListingRentId(long listingRentId);
        Task<List<TCity>> Find(string filter);
        Task<List<TCity>> FindByFilter(string filter, int filterType);
        Task<List<TCountry>> SearchCountries(string filter);
        Task<List<TState>> SearchStates(string filter);
        Task<List<TCounty>> SearchCounties(string filter);
        Task<List<TCity>> SearchCities(string filter);

    }
}
