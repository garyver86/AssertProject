using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayPayoutTransaction
{
    public long PayoutTransactionId { get; set; }

    public long PriceCalculationId { get; set; }

    public int HostId { get; set; }

    public int PayoutAccountId { get; set; }

    public int MethodOfPaymentId { get; set; }

    public decimal Amount { get; set; }

    public string PayoutStatus { get; set; } = null!;

    public string? PayoutType { get; set; }

    public DateTime ScheduledPayoutDate { get; set; }

    public DateTime? PayoutDate { get; set; }

    public int AttemptCount { get; set; }

    public string? FailureReason { get; set; }

    public virtual ICollection<PayPaymentHold> PayPaymentHolds { get; set; } = new List<PayPaymentHold>();

    public virtual PayPayoutAccount PayoutAccount { get; set; } = null!;

    public virtual PayPriceCalculation PriceCalculation { get; set; } = null!;
}
