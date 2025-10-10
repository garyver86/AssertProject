using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookCancellationReason
{
    public int CancellationReasonId { get; set; }

    public int? CancellationReasonParentId { get; set; }

    public int? CancellationGroupId { get; set; }

    public string? CancellationTypeCode { get; set; }

    public int? CancellationLevel { get; set; }

    public bool? IsEndStep { get; set; }

    public string? Title { get; set; }

    public string? Detail { get; set; }

    public string? MessageTo { get; set; }

    public string? Icon { get; set; }

    public string? Status { get; set; }

    public virtual TbBookCancellationGroup? CancellationGroup { get; set; }

    public virtual ICollection<TbBookCancellation> TbBookCancellations { get; set; } = new List<TbBookCancellation>();
}
