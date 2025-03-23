using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingStatusRepository
    {
        Task<TlListingStatus> Get(int id);
        Task<TlListingStatus> Get(string code);
    }
}
