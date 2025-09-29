using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TPreparationTimeType
{
    public int PreparationTimeTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Value { get; set; }

    public int? Status { get; set; }
}
