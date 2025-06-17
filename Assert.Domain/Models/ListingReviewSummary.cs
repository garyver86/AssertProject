using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class ListingReviewSummary
    {
        public long ListingRentId { get; set; }
        public List<TlListingReview> TopReviews { get; set; }
        public List<TlListingReview> BottomReviews { get; set; }
        public List<TlListingReview> RecentReviews { get; set; }
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
    }
}
