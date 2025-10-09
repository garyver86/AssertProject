using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookCancellationGroup
{
    public int CancellationGroupId { get; set; }

    public string? Code { get; set; }

    public string? Title { get; set; }

    public string? Detail { get; set; }

    public int? Position { get; set; }

    public string? Icon { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TbBookCancellationReason> TbBookCancellationReasons { get; set; } = new List<TbBookCancellationReason>();
}
