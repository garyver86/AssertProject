using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookStep
{
    public int BookStepId { get; set; }

    public long BookId { get; set; }

    public int BookStepTypeViewId { get; set; }

    public bool IsEnded { get; set; }

    public int BookStepStatusId { get; set; }

    public virtual TbBook Book { get; set; } = null!;

    public virtual TbBookStepStatus BookStepStatus { get; set; } = null!;

    public virtual TbBookStepType BookStepTypeView { get; set; } = null!;
}
