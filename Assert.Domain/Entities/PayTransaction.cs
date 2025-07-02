using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayTransaction
{
    public long PayTransactionId { get; set; }

    public string OrderCode { get; set; } = null!;

    public string Stan { get; set; } = null!;

    public long? BookingId { get; set; }

    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public int MethodOfPaymentId { get; set; }

    public int PaymentProviderId { get; set; }

    public int CountryId { get; set; }

    public string TransactionStatusCode { get; set; } = null!;

    public string TransactionStatus { get; set; } = null!;

    public string? PaymentData { get; set; }

    public string? TransactionData { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? CupdatedAt { get; set; }

    public virtual TCountry Country { get; set; } = null!;

    public virtual PayMethodOfPayment MethodOfPayment { get; set; } = null!;

    public virtual ICollection<PayPriceCalculation> PayPriceCalculations { get; set; } = new List<PayPriceCalculation>();

    public virtual PayProvider PaymentProvider { get; set; } = null!;
}
