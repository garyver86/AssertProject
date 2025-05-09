using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingStepsStatus
{
    public int ListingStepsStatusId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TlListingStep> TlListingSteps { get; set; } = new List<TlListingStep>();
}
