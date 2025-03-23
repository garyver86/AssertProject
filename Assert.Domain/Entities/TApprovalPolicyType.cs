using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TApprovalPolicyType
{
    public int ApprovalPolicyTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? IsRecommended { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<TlListingRent> TlListingRents { get; set; } = new List<TlListingRent>();
}
