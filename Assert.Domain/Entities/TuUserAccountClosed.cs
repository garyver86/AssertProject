using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserAccountClosed
{
    public int UserAccountClosedId { get; set; }

    public int? UserId { get; set; }

    public int? UserSelectionOptionsId { get; set; }

    public DateTime? ClosingDate { get; set; }

    public DateTime? OpeningDate { get; set; }

    public string? Status { get; set; }

    public virtual TuUser? User { get; set; }

    public virtual TuUserSelectionOption? UserSelectionOptions { get; set; }
}
