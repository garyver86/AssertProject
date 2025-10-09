using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookCancellationType
{
    public int BookCancellationTypeId { get; set; }

    public string? CanellationTypeCode { get; set; }

    public string? Name { get; set; }

    public string? Detalle { get; set; }

    public string? Status { get; set; }
}
