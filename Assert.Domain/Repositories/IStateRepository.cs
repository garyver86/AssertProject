using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IStateRepository
    {
        Task<TState> GetByCityId(long cityId);
        Task<IEnumerable<TState>> Find(string filter);

    }
}
