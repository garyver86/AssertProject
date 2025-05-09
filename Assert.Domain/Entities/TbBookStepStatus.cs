using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookStepStatus
{
    public int BookStepStatusId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<TbBookStep> TbBookSteps { get; set; } = new List<TbBookStep>();
}
