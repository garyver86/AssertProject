using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TReasonRefusedBook
{
    public int ReasonRefusedId { get; set; }

    public string ReasonRefusedCode { get; set; } = null!;

    public string ReasonRefusedName { get; set; } = null!;

    public string? ReasonRefusedText { get; set; }

    public virtual ICollection<TbBook> TbBooks { get; set; } = new List<TbBook>();
}
