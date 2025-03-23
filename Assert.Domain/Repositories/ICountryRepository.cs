using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ICountryRepository
    {
        Task<TCountry> GetByStateId(long stateId);
    }
}
