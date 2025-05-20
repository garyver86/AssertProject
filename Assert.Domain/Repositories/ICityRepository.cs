using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ICityRepository
    {
        TCity GetById(int cityId);
        Task<TCity> GetByListingRentId(long listingRentId);
        Task<List<TCity>> Find(string filter);
        Task<List<TCity>> FindByFilter(string filter, int filterType);

    }
}
