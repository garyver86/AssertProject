using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCounty
{
    public long CountyId { get; set; }

    public string Name { get; set; } = null!;

    public long? StateId { get; set; }

    public virtual TState? State { get; set; }

    public virtual ICollection<TCity> TCities { get; set; } = new List<TCity>();
}
