using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpRuleType
{
    public int RuleTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public string? IconLink { get; set; }

    public virtual ICollection<TlListingRentRule> TlListingRentRules { get; set; } = new List<TlListingRentRule>();
}
