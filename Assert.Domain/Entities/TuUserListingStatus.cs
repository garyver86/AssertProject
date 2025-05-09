using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserListingStatus
{
    public int UserListingStatusId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TuUserListingRent> TuUserListingRents { get; set; } = new List<TuUserListingRent>();
}
