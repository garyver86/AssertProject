using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuAccountType
{
    public int AccountTypeId { get; set; }

    public string AccountTypeCode { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public virtual ICollection<TuUser> TuUsers { get; set; } = new List<TuUser>();
}
