using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayPriceCalculationStatus
{
    public int PayPriceCalculationStatus1 { get; set; }

    public string PriceCalculationStatusCode { get; set; } = null!;

    public string PriceCalculationStatusDescription { get; set; } = null!;

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();
}
