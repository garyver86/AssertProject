using Assert.Domain.Entities;
using System.Diagnostics.Metrics;

namespace Assert.Domain.Repositories
{
    public interface ICountyRepository
    {
        Task<IEnumerable<TCounty>> Find(string filter);

    }
}
