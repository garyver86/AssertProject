using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserReview
{
    public long UserReviewId { get; set; }

    public int UserId { get; set; }

    public long? ListingRentId { get; set; }

    public long? BookId { get; set; }

    public DateTime? DateTimeReview { get; set; }

    public int UserIdReviewer { get; set; }

    public int Calification { get; set; }

    public string? Comment { get; set; }

    public int? Status { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TuUser User { get; set; } = null!;

    public virtual TuUser UserIdReviewerNavigation { get; set; } = null!;
}
