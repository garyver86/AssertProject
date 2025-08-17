using Assert.Domain.Entities;
using Assert.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Assert.Domain.Repositories
{
    public interface ICityRepository
    {
        Task<TCity> GetById(int cityId);
        Task<TCity> GetByListingRentId(long listingRentId);
        Task<List<TCity>> Find(string filter);
        Task<List<TCity>> FindByFilter(string filter, int filterType);
        Task<List<TCountry>> SearchCountries(string filter);
        Task<List<TState>> SearchStates(string filter);
        Task<List<TCounty>> SearchCounties(string filter);
        Task<List<TCity>> SearchCities(string filter);
        Task<TCounty> GetCountyById(int countyId);
        Task<TState> GetStateById(int stateId);
        Task<TCountry> GetCountryById(int countryId);
        Task<int?> FindBestCountryMatch(string normalizedCountryName);
        Task<int?> FindBestStateMatch(string normalizedStateName, int? countryId);
        Task<int?> FindBestCountyMatch(string normalizedCountyName, int? stateId);
        Task<int?> FindBestCityMatch(string normalizedCityName, int? countyId);
        TCity RegisterLocation(string country, string state, string county, string city, string? street, LocationModel result);
    }
}
