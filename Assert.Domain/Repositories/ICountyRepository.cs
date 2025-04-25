using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ICountyRepository
    {
        Task<IEnumerable<TCounty>> Find(string filter);

    }
}
