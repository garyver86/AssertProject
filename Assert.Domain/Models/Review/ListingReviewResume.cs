using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models.Review
{
    public class ListingReviewResume
    {
        public ListingResume Listing { get; set; }
        public List<AvgByQuestion> AvgByQuestion { get; set; }
    }
    public class ListingResume
    {
        public long ListingRentId { get; set; }
        public string Name { get; set; }
        public decimal? AvgReviews { get; set; }
        public int TotalReviews { get; set; }
    }
    public class AvgByQuestion
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionCode { get; set; }
        public decimal AverageRating { get; set; }
        public int TotalAnswers { get; set; }
    }
}
