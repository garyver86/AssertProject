using Assert.Domain.Entities;

namespace Assert.Application.DTOs.Responses;

public class ReviewDTO
{
    public string User { get; set; }
    public DateTime? DateTimeReview { get; set; }
    public int Calification { get; set; }
    public string? Comment { get; set; }
    public string UserProfilePhoto { get; set; }
    public bool? IsComplete { get; set; }
    public long ListingReviewId { get; set; }
    public int UserId { get; set; }
    public long ListingRentId { get; set; }
    public long? BookId { get; set; }
    public virtual ICollection<ReviewQuestionAnswerDTO> ReviewQuestions { get; set; } = new List<ReviewQuestionAnswerDTO>();
}
public class ReviewQuestionDTO
{
    public int ReviewQuestionId { get; set; }
    public string? QuestionText { get; set; }
    public string QuestionCode { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime? CreationDate { get; set; }
}
public class ReviewQuestionAnswerDTO
{
    public long ListingReviewQuestionId { get; set; }
    public long ListingReviewId { get; set; }
    public int ReviewQuestionId { get; set; }
    public int Rating { get; set; }
    public DateTime ReviewDate { get; set; }
}