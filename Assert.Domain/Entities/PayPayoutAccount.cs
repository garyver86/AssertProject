using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayPayoutAccount
{
    public int PayoutAccountId { get; set; }

    public int UserId { get; set; }

    public string NumberAccount { get; set; } = null!;

    public string? AccountDetails { get; set; }

    public int MethodOfPayment { get; set; }

    public string AccStatus { get; set; } = null!;

    public bool IsPrincipal { get; set; }

    public virtual PayMethodOfPayment MethodOfPaymentNavigation { get; set; } = null!;

    public virtual ICollection<PayPayoutTransaction> PayPayoutTransactions { get; set; } = new List<PayPayoutTransaction>();

    public virtual TuUser User { get; set; } = null!;
}
