namespace Assert.Domain.Entities;

public partial class TiIssueType
{
    public int IssueTypeId { get; set; }

    public string IssueName { get; set; } = null!;

    public string? IssueCode { get; set; }

    public virtual ICollection<TiIssue> TiIssues { get; set; } = new List<TiIssue>();
}
