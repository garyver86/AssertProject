using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCancelationPolicyType
{
    public int CancelationPolicyTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Detail1 { get; set; }

    public int? RefoundUpToPercentage { get; set; }

    public int? RefoundUpToDays { get; set; }

    public string? Detail2 { get; set; }

    public int? DiscountPercentage { get; set; }

    public int? WithinDays { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<TlListingRent> TlListingRents { get; set; } = new List<TlListingRent>();
}
