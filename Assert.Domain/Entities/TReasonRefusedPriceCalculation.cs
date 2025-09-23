using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TReasonRefusedPriceCalculation
{
    public int ReasonRefusedId { get; set; }

    public string ReasonRefusedCode { get; set; } = null!;

    public string ReasonRefusedName { get; set; } = null!;

    public string? ReasonRefusedText { get; set; }

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();
}
