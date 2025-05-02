using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ICountryRepository
    {
        Task<TCountry> GetByStateId(long stateId);

        Task<IEnumerable<TCountry>> Find(string filter);

    }
}
