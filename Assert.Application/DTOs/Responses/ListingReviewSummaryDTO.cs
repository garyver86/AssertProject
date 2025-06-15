using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ListingReviewSummaryDTO
    {
        public long ListingRentId { get; set; }
        public List<ReviewDTO> TopReviews { get; set; }
        public List<ReviewDTO> BottomReviews { get; set; }
        public List<ReviewDTO> RecentReviews { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
