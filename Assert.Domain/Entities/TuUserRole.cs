using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserRole
{
    public int UserRoleId { get; set; }

    public int UserId { get; set; }

    public int UserTypeId { get; set; }

    public bool? IsActive { get; set; }

    public virtual TuUser User { get; set; } = null!;

    public virtual TuUserType UserType { get; set; } = null!;
}
