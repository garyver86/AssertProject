using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserSelectionOption
{
    public int UserSelectionOptionsId { get; set; }

    public int? UserGroupTypeId { get; set; }

    public string? ToUse { get; set; }

    public string? OptionDetail { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<TuUserAccountClosed> TuUserAccountCloseds { get; set; } = new List<TuUserAccountClosed>();

    public virtual TuUserGroupType? UserGroupType { get; set; }
}
