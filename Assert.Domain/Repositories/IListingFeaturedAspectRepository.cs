
using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingFeaturedAspectRepository
    {
        Task SetListingFeaturesAspects(long listingRentId, List<int> featuredAspects);
        Task<List<TlListingFeaturedAspect>?> GetByListingRentId(long listingRentId);
    }
}
