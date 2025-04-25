namespace Assert.Domain.Entities;

public partial class TuUserReview
{
    public long UserReviewId { get; set; }

    public int UserId { get; set; }

    public DateTime? DateTimeReview { get; set; }

    public int UserIdReviewer { get; set; }

    public int Calification { get; set; }

    public string? Comment { get; set; }

    public virtual TuUser User { get; set; } = null!;

    public virtual TuUser UserIdReviewerNavigation { get; set; } = null!;
}
