using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserGroupType
{
    public int UserGroupTypeId { get; set; }

    public string? ToUse { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TuUserSelectionOption> TuUserSelectionOptions { get; set; } = new List<TuUserSelectionOption>();
}
