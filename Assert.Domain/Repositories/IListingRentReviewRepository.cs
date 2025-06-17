using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IListingRentReviewRepository
    {
        Task<List<TlListingReview>> GetByListingRent(long listingRentId);
        Task<ListingReviewSummary> GetReviewSummary(long listingRentId, int topCount);
    }
}
