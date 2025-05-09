using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookStatus
{
    public int BookStatusId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string? Code { get; set; }

    public virtual ICollection<TbBook> TbBooks { get; set; } = new List<TbBook>();
}
