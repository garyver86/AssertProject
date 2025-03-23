using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TiStatusIssue
{
    public int StatusIssueId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? StatusDescription { get; set; }

    public virtual ICollection<TiIssue> TiIssues { get; set; } = new List<TiIssue>();
}
