using Assert.Domain.Entities;
using System.Diagnostics.Metrics;

namespace Assert.Domain.Repositories
{
    public interface ICountryRepository
    {
        Task<TCountry> GetByStateId(long stateId);

        Task<IEnumerable<TCountry>> Find(string filter);

    }
}
