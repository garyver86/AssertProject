using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingSpaceRepository
    {
        Task<List<TlListingSpace>>? GetByListingRentId(long listingRentId);
    }
}
