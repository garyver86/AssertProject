using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingReviewQuestion
{
    public long ListingReviewQuestionId { get; set; }

    public long ListingReviewId { get; set; }

    public int ReviewQuestionId { get; set; }

    public int Rating { get; set; }

    public DateTime ReviewDate { get; set; }

    public virtual TlListingReview ListingReview { get; set; } = null!;

    public virtual TReviewQuestion ReviewQuestion { get; set; } = null!;
}
