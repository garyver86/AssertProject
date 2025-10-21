using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayPaymentHold
{
    public long PaymentHoldId { get; set; }

    public int HostId { get; set; }

    public long PayoutTransactionId { get; set; }

    public string Reason { get; set; } = null!;

    public string PaymentHoldStatus { get; set; } = null!;

    public int PaymentHoldStatusId { get; set; }

    public string? Notes { get; set; }

    public int? ReasonId { get; set; }

    public virtual PayPayoutTransaction PayoutTransaction { get; set; } = null!;
}
