using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IReviewService
    {
        //Task<TlListingReview> GetReviewStatusAsync(int bookId, int userId);
        Task<TlListingReview> GetReviewDetailsAsync(long listingReviewId);
        Task<List<TReviewQuestion>> GetReviewsQuestions();
        Task<TlListingReview> SubmitReviewAsync(TlListingReview reviewDto, int userId);
    }
}
