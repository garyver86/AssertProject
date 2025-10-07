using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Models.Review;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    internal class ListingRentReviewRepository : IListingRentReviewRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly DbContextOptions<InfraAssertDbContext> dbOptions;
        private readonly IReviewQuestionRepository _questionRepository;

        public ListingRentReviewRepository(InfraAssertDbContext infraAssertDbContext, IServiceProvider serviceProvider,
            IReviewQuestionRepository questionRepository)
        {
            _context = infraAssertDbContext;
            _questionRepository = questionRepository;
            dbOptions = serviceProvider.GetRequiredService<DbContextOptions<InfraAssertDbContext>>();
        }
        public async Task<List<TlListingReview>> GetByListingRent(long listingRentId)
        {
            var result = await _context.TlListingReviews
                .Include(x => x.User)
                .Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }

        public async Task<TlListingReview> RegisterReview(long listingRentId, int calification, string comment, int userId)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                TlListingReview review = new TlListingReview
                {
                    ListingRentId = listingRentId,
                    UserId = userId,
                    DateTimeReview = DateTime.UtcNow,
                    Calification = calification,
                    Comment = comment
                };
                dbContext.TlListingReviews.Add(review);
                await dbContext.SaveChangesAsync();

                UpdateReviewsAverage(listingRentId);

                return review;
            }
        }

        public async Task UpdateReviewsAverage(long listingRentId)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                var averageRating = await dbContext.TlListingReviews
                    .Where(r => r.ListingRentId == listingRentId)
                    .AverageAsync(r => r.Calification);

                TlListingRent listing = await dbContext.TlListingRents
                    .FirstOrDefaultAsync(l => l.ListingRentId == listingRentId);

                listing.AvgReviews = (Decimal)(averageRating);
                await dbContext.SaveChangesAsync();
                return;
            }
        }

        public async Task<ListingReviewResume> GetreviewAverageByListing(long listingRentId)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                var result = await dbContext.TlListingRents.Where(li => li.ListingRentId == listingRentId).
                    Select(p => new ListingReviewResume
                    {
                        Listing = new ListingResume
                        {
                            ListingRentId = p.ListingRentId,
                            Name = p.Name,
                            AvgReviews = p.AvgReviews,
                            TotalReviews = p.TlListingReviews.Count()
                        },
                        AvgByQuestion = p.TlListingReviews
                            .SelectMany(r => r.TlListingReviewQuestions)
                            .GroupBy(q => new
                            {
                                q.ReviewQuestionId,
                                q.ReviewQuestion.QuestionCode,
                                q.ReviewQuestion.QuestionText
                            })
                            .Select(g => new AvgByQuestion
                            {
                                QuestionId = g.Key.ReviewQuestionId,
                                QuestionText = g.Key.QuestionText,
                                QuestionCode = g.Key.QuestionCode,
                                AverageRating = (decimal)g.Average(q => q.Rating),
                                TotalAnswers = g.Count()
                            }).ToList()
                    }).FirstOrDefaultAsync();
                if (result.Listing.AvgReviews == null)
                {
                    await UpdateReviewsAverage(listingRentId);
                    result.Listing.AvgReviews = (decimal)await dbContext.TlListingReviews
                    .Where(r => r.ListingRentId == listingRentId)
                    .AverageAsync(r => r.Calification);
                }
                return result;
            }
        }

        public async Task<ListingReviewSummary> GetReviewSummary(long listingRentId, int topCount)
        {
            if (topCount <= 0)
            {
                throw new ArgumentException("El número de reviews debe ser mayor que cero", nameof(topCount));
            }

            // Consulta base para obtener todas las reviews del listing
            var baseQuery = _context.TlListingReviews
                .Where(r => r.ListingRentId == listingRentId)
                .Include(r => r.User) // Incluir información del usuario que hizo la review
                .AsQueryable();

            // Obtener las mejores reviews (mayor calificación)
            var topReviews = await baseQuery
                .OrderByDescending(r => r.Calification)
                .ThenByDescending(r => r.DateTimeReview)
                .Take(topCount)
                //.Select(r => new ReviewDto
                //{
                //    Id = r.ListingReviewId,
                //    Calification = r.Calification,
                //    Comment = r.Comment,
                //    Date = r.DateTimeReview,
                //    UserName = r.User.Name,
                //    UserPhoto = r.User.PhotoLink
                //})
                .ToListAsync();

            // Obtener las peores reviews (menor calificación)
            var bottomReviews = await baseQuery
                .OrderBy(r => r.Calification)
                .ThenByDescending(r => r.DateTimeReview)
                .Take(topCount)
                //.Select(r => new ReviewDto
                //{
                //    Id = r.ListingReviewId,
                //    Calification = r.Calification,
                //    Comment = r.Comment,
                //    Date = r.DateTimeReview,
                //    UserName = r.User.Name,
                //    UserPhoto = r.User.PhotoLink
                //})
                .ToListAsync();

            // Obtener las reviews más recientes
            var recentReviews = await baseQuery
                .OrderByDescending(r => r.DateTimeReview)
                .Take(topCount)
                //.Select(r => new ReviewDto
                //{
                //    Id = r.ListingReviewId,
                //    Calification = r.Calification,
                //    Comment = r.Comment,
                //    Date = r.DateTimeReview,
                //    UserName = r.User.Name,
                //    UserPhoto = r.User.PhotoLink
                //})
                .ToListAsync();

            // Calcular estadísticas adicionales
            var stats = await baseQuery
                .GroupBy(r => 1)
                .Select(g => new
                {
                    AverageRating = g.Average(r => (double)r.Calification),
                    TotalReviews = g.Count()
                })
                .FirstOrDefaultAsync();

            return new ListingReviewSummary
            {
                ListingRentId = listingRentId,
                TopReviews = topReviews,
                BottomReviews = bottomReviews,
                RecentReviews = recentReviews,
                AverageRating = stats?.AverageRating ?? 0,
                TotalReviews = stats?.TotalReviews ?? 0
            };
        }


        public async Task<TlListingReview?> GetReviewByBookingAsync(long bookId)
        {
            return await _context.TlListingReviews
                .Include(r => r.TlListingReviewQuestions)
                .ThenInclude(q => q.ReviewQuestion)
                .Include(lr => lr.ListingRent)
                .FirstOrDefaultAsync(r => r.BookId == bookId);
        }

        public async Task<TlListingReview?> GetReviewByIdAsync(long listingReviewId)
        {
            return await _context.TlListingReviews
                .Include(r => r.TlListingReviewQuestions)
                .ThenInclude(q => q.ReviewQuestion)
                .FirstOrDefaultAsync(r => r.ListingReviewId == listingReviewId);
        }

        public async Task<TlListingReview> CreateReviewAsync(TlListingReview review)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                dbContext.TlListingReviews.Add(review);
                await dbContext.SaveChangesAsync();
                return review;
            }
        }

        public async Task UpdateReviewAsync(TlListingReview review)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                TlListingReview update = new TlListingReview
                {
                    BookId = review.BookId,
                    Calification = review.Calification,
                    Comment = review.Comment,
                    DateTimeReview = review.DateTimeReview,
                    IsComplete = review.IsComplete,
                    ListingRentId = review.ListingRentId,
                    ListingReviewId = review.ListingReviewId,
                    UserId = review.UserId
                };
                dbContext.TlListingReviews.Update(update);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task SaveAnswersAsync(List<TlListingReviewQuestion> answers)
        {
            using (var dbContext = new InfraAssertDbContext(dbOptions))
            {
                foreach (var answer in answers)
                {
                    var existing = await dbContext.TlListingReviewQuestions
                        .FirstOrDefaultAsync(a =>
                            a.ListingReviewId == answer.ListingReviewId &&
                            a.ReviewQuestionId == answer.ReviewQuestionId);

                    if (existing != null && existing.Rating != answer.Rating)
                    {
                        existing.Rating = answer.Rating;
                        existing.ReviewDate = DateTime.UtcNow;
                        dbContext.TlListingReviewQuestions.Update(existing);
                    }
                    else if (existing == null)
                    {
                        answer.ReviewDate = DateTime.UtcNow;
                        dbContext.TlListingReviewQuestions.Add(answer);
                    }
                }

                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalActiveQuestionsAsync()
        {
            return await _context.TReviewQuestions
                .CountAsync(q => q.IsActive);
        }

        public async Task<int> GetAnsweredQuestionsCountAsync(long bookId)
        {
            return await _context.TlListingReviewQuestions
                .Include(a => a.ListingReview)
                .Where(a => a.ListingReview.BookId == bookId)
                .CountAsync();
        }
    }
}
