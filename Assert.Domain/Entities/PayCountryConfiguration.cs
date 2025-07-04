using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayCountryConfiguration
{
    public int PaymentConfigId { get; set; }

    public int CountryId { get; set; }

    public int MethodOfPaymentId { get; set; }

    public int ProviderId { get; set; }

    public int Priority { get; set; }

    public bool Active { get; set; }

    public string? ConfigurationJson { get; set; }

    public virtual TCountry Country { get; set; } = null!;

    public virtual PayMethodOfPayment MethodOfPayment { get; set; } = null!;

    public virtual PayProvider Provider { get; set; } = null!;
}
