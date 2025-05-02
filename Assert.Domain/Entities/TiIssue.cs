namespace Assert.Domain.Entities;

public partial class TiIssue
{
    public int IssueId { get; set; }

    public int ReportedByUserId { get; set; }

    public long? BookingId { get; set; }

    public int? RelatedUserId { get; set; }

    public int IssueTypeId { get; set; }

    public string DescriptionIssue { get; set; } = null!;

    public int StatusIssueId { get; set; }

    public DateTime? ReportedDate { get; set; }

    public DateTime? ResolvedDate { get; set; }

    public int? Priority { get; set; }

    public virtual TbBook? Booking { get; set; }

    public virtual TiIssueType IssueType { get; set; } = null!;

    public virtual TuUser? RelatedUser { get; set; }

    public virtual TuUser ReportedByUser { get; set; } = null!;

    public virtual TiStatusIssue StatusIssue { get; set; } = null!;
}
