using Assert.Domain.Entities;
using Assert.Domain.Models;

namespace Assert.Domain.Repositories
{
    public interface IListingRentReviewRepository
    {
        Task<List<TlListingReview>> GetByListingRent(long listingRentId);
        Task<ListingReviewSummary> GetReviewSummary(long listingRentId, int topCount);
        Task<TlListingReview> RegisterReview(long listingRentId, int calification, string comment, int userId);

        Task<TlListingReview?> GetReviewByBookingAsync(long bookId);
        Task<TlListingReview?> GetReviewByIdAsync(long listingReviewId);
        Task<TlListingReview> CreateReviewAsync(TlListingReview review);
        Task UpdateReviewAsync(TlListingReview review);
        Task SaveAnswersAsync(List<TlListingReviewQuestion> answers);
        Task<int> GetTotalActiveQuestionsAsync();
        Task<int> GetAnsweredQuestionsCountAsync(long bookid);
        Task UpdateReviewsAverage(long listingRentId);
    }
}
