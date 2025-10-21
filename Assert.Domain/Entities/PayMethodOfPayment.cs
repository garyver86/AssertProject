using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayMethodOfPayment
{
    public int MethodOfPaymentId { get; set; }

    public string MopName { get; set; } = null!;

    public string? MopDescription { get; set; }

    public bool Active { get; set; }

    public string? UrlIcon { get; set; }

    public string? MopCode { get; set; }

    public virtual ICollection<PayAccount> PayAccounts { get; set; } = new List<PayAccount>();

    public virtual ICollection<PayCountryConfiguration> PayCountryConfigurations { get; set; } = new List<PayCountryConfiguration>();

    public virtual ICollection<PayPayoutAccount> PayPayoutAccounts { get; set; } = new List<PayPayoutAccount>();

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();

    public virtual ICollection<PayTransaction> PayTransactions { get; set; } = new List<PayTransaction>();
}
