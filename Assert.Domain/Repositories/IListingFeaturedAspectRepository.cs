
namespace Assert.Domain.Repositories
{
    public interface IListingFeaturedAspectRepository
    {
        Task SetListingFeaturesAspects(long listingRentId, List<int> featuredAspects);
    }
}
