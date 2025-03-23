using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpPropertyType
{
    public int PropertyTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<TpPropertySubtype> TpPropertySubtypes { get; set; } = new List<TpPropertySubtype>();
}
