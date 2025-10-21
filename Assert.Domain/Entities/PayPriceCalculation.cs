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

    public int CalculationStatusId { get; set; }

    public bool? ConsultAccepted { get; set; }

    public DateTime? ConsultResponse { get; set; }

    public int? ReasonRefusedId { get; set; }

    public int? UserId { get; set; }

    public int? Guests { get; set; }

    public bool? ExistPet { get; set; }

    public decimal? AdditionalFees { get; set; }

    public decimal? Discounts { get; set; }

    public decimal? PlatformFee { get; set; }

    public DateTime? DatetimePayment { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual PayPriceCalculationStatus CalculationStatus { get; set; } = null!;

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual PayMethodOfPayment? MethodOfPayment { get; set; }

    public virtual ICollection<PayPayoutTransaction> PayPayoutTransactions { get; set; } = new List<PayPayoutTransaction>();

    public virtual PayProvider? PaymentProvider { get; set; }

    public virtual PayTransaction? PaymentTransaction { get; set; }

    public virtual TReasonRefusedPriceCalculation? ReasonRefused { get; set; }

    public virtual ICollection<TmConversation> TmConversations { get; set; } = new List<TmConversation>();

    public virtual TuUser? User { get; set; }
}
