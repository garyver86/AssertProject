namespace Assert.Application.DTOs.Responses;

public class UserReviewDTO
{
    public long ListingReviewId { get; set; }
    public int UserId { get; set; }
    public DateTime? DateTimeReview { get; set; }
    public long ListingRentId { get; set; }
    public long? BookId { get; set; }
    public int UserIdReviewer { get; set; }
    public int Calification { get; set; }
    public string? Comment { get; set; }
}
