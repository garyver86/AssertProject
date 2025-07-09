using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayProvider
{
    public int ProviderId { get; set; }

    public string ProviderName { get; set; } = null!;

    public string? ProviderDescription { get; set; }

    public string? ApiUrl { get; set; }

    public bool Active { get; set; }

    public string? IntegrationConfiguration { get; set; }

    public string? ResponseType { get; set; }

    public string? ProviderCode { get; set; }

    public virtual ICollection<PayCountryConfiguration> PayCountryConfigurations { get; set; } = new List<PayCountryConfiguration>();

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();

    public virtual ICollection<PayTransaction> PayTransactions { get; set; } = new List<PayTransaction>();
}
