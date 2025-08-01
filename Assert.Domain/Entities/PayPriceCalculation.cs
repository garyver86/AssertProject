using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayPriceCalculation
{
    public long PriceCalculationId { get; set; }

    public Guid? CalculationCode { get; set; }

    public long? BookId { get; set; }

    public decimal Amount { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public string? CalculationDetails { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string CalculationStatue { get; set; } = null!;

    public string? IpAddress { get; set; }

    public string? UserAgent { get; set; }

    public int? MethodOfPaymentId { get; set; }

    public int? PaymentProviderId { get; set; }

    public long? PaymentTransactionId { get; set; }

    public long? ListingRentId { get; set; }

    public DateTime? InitBook { get; set; }

    public DateTime? EndBook { get; set; }

    public string? BreakdownInfo { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual PayMethodOfPayment? MethodOfPayment { get; set; }

    public virtual PayProvider? PaymentProvider { get; set; }

    public virtual PayTransaction? PaymentTransaction { get; set; }
}
