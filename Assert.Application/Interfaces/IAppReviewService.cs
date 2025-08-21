using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.Interfaces
{
    public interface IAppReviewService
    {
        Task<ReturnModelDTO<ReviewDTO>> GetBookReviewDetails(long bookId, Dictionary<string, string> clientData);
        Task<ReturnModelDTO<ReviewDTO>> SubmitBookReview(ReviewDTO reviewDto, int userId, Dictionary<string, string> clientData);
        Task<ReturnModelDTO<List<ReviewQuestionDTO>>> GetReviewQuestions(Dictionary<string, string> clientData);
        Task<ReturnModelDTO<ListingReviewResumeDTO>> GetreviewAverageByListing(long listingId, Dictionary<string, string> clientData);
    }
}
