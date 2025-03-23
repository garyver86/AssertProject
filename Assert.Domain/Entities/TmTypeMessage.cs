using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TmTypeMessage
{
    public int TypeMessageId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<TmMessage> TmMessages { get; set; } = new List<TmMessage>();
}
