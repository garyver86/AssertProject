using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingRentReviewRepository
    {
        Task<List<TlListingReview>> GetByListingRent(long listingRentId);
    }
}
