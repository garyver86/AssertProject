using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserStatusType
{
    public int UserStatusTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TuUser> TuUsers { get; set; } = new List<TuUser>();
}
