using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface ICityRepository
    {
        TCity GetById(int cityId);
        Task<TCity> GetByListingRentId(long listingRentId);
    }
}
