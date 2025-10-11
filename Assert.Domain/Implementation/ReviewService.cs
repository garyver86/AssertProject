using Assert.Domain.Entities;
using Assert.Domain.Models.Review;
using Assert.Domain.Repositories;
using Assert.Domain.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IListingRentReviewRepository _reviewRepository;
        private readonly IReviewQuestionRepository _questionRepository;
        private readonly IBookRepository _bookRepository;

        public ReviewService(
            IListingRentReviewRepository reviewRepository,
            IReviewQuestionRepository questionRepository,
            IBookRepository bookRepository)
        {
            _reviewRepository = reviewRepository;
            _questionRepository = questionRepository;
            _bookRepository = bookRepository; ;
        }

        private async Task<decimal> CalculateReviewAverageAsync(long listingReviewId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(listingReviewId);
            if (review == null || !review.TlListingReviewQuestions.Any())
                return 0;

            return (decimal)review.TlListingReviewQuestions.Average(a => a.Rating);
        }

        public async Task<TlListingReview> GetReviewDetailsAsync(long bookId)
        {
            var review = await _reviewRepository.GetReviewByBookingAsync(bookId);
            //if (review == null)
            //    throw new KeyNotFoundException("Review not found");

            return review;
        }
        public async Task<ListingReviewResume> GetreviewAverageByListing(long listingId)
        {
            var review = await _reviewRepository.GetreviewAverageByListing(listingId);

            return review;
        }

        public async Task<TlListingReview> SubmitReviewAsync(TlListingReview reviewDto, int userId)
        {
            var bookInfo = await _bookRepository.GetByIdAsync(reviewDto.BookId ?? 0);

            if (bookInfo == null || bookInfo.UserIdRenter != userId)
            {
                throw new ArgumentException($"No tiene permisos para realizar un review sobre esta reserva.");
            }

            // Validar respuestas
            var questions = await _questionRepository.GetActiveQuestionsAsync();
            var activeQuestionIds = questions.Select(q => q.ReviewQuestionId).ToHashSet();

            foreach (var answer in reviewDto.TlListingReviewQuestions)
            {
                if (!activeQuestionIds.Contains(answer.ReviewQuestionId))
                    throw new ArgumentException($"Question ID {answer.ReviewQuestionId} is not active or doesn't exist");

                if (answer.Rating < 1 || answer.Rating > 5)
                    throw new ArgumentException($"Rating for question {answer.ReviewQuestionId} must be between 1 and 5");
            }

            // Obtener o crear review
            var review = await _reviewRepository.GetReviewByBookingAsync(reviewDto.BookId ?? 0);
            var isNewReview = review == null;

            if (isNewReview)
            {
                review = new TlListingReview
                {
                    UserId = userId,
                    DateTimeReview = DateTime.UtcNow,
                    ListingRentId = bookInfo.ListingRentId,
                    Calification = 0,
                    Comment = reviewDto.Comment,
                    BookId = reviewDto.BookId,
                    IsComplete = false
                };

                review = await _reviewRepository.CreateReviewAsync(review);
            }
            else if (reviewDto.Comment != null && reviewDto.Comment != "" && review.Comment != reviewDto.Comment)
            {
                review.Comment = reviewDto.Comment;
            }

            foreach (var ans in reviewDto.TlListingReviewQuestions)
            {
                ans.ListingReviewId = review.ListingReviewId;
            }

            // Guardar respuestas
            var answers = reviewDto.TlListingReviewQuestions.Select(x => new TlListingReviewQuestion
            {
                ListingReviewId = x.ListingReviewId,
                ListingReviewQuestionId = x.ListingReviewQuestionId,
                ReviewDate = x.ReviewDate,
                Rating = x.Rating,
                ReviewQuestionId = x.ReviewQuestionId
            }).ToList();

            if (answers.Count > 0)
            {
                await _reviewRepository.SaveAnswersAsync(answers);
            }

            // Calcular promedio y actualizar review
            var average = await CalculateReviewAverageAsync(review.ListingReviewId);
            var decimalPart = average - Math.Truncate(average);

            // Redondear hacia arriba si la parte decimal es >= 0.6
            int prom = decimalPart >= 0.6m ? (int)Math.Ceiling(average) : (int)Math.Floor(average);
            review.Calification = prom;
            _reviewRepository.UpdateReviewsAverage(bookInfo.ListingRentId);
            await _reviewRepository.UpdateReviewAsync(review);

            var result = await GetReviewDetailsAsync(review.BookId ?? 0);
            if (!(result.IsComplete ?? false) && result.TlListingReviewQuestions?.Count == questions.Count && !(result.Comment.IsNullOrEmpty()))
            {
                review.IsComplete = true;
                _reviewRepository.UpdateReviewAsync(review);
                _bookRepository.SetReviewDateTime(bookInfo.BookId);
                result.IsComplete = review.IsComplete;
            }
            return result;
        }

        public async Task<List<TReviewQuestion>> GetReviewsQuestions()
        {
            var result = await _questionRepository.GetActiveQuestionsAsync();
            return result;
        }
    }
}
