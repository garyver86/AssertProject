namespace Assert.Domain.Models.Review;

public class CommonReview
{
    public long ReviewId { get; set; }
    public long ListingRentId { get; set; }
    public long BookId { get; set; }
    public int UserIdReviewer { get; set; }
    public DateTime DateTimeReview { get; set; }

    public string ReviewerName { get; set; } = string.Empty;
    public string ReviewerLocation { get; set; } = string.Empty;
    public string ReviewDateName { get; set; } = string.Empty;
    public int StayDuration { get; set; }
    public double Rating { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
}
