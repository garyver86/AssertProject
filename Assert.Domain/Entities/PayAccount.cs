using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class PayAccount
{
    public int PayAccountId { get; set; }

    public int MethodOfPaymentId { get; set; }

    public string NumberAccount { get; set; } = null!;

    public string? AccountToken { get; set; }

    public string? AccountDetails { get; set; }

    public string AccountType { get; set; } = null!;

    public string AccStatus { get; set; } = null!;

    public int UserId { get; set; }

    public virtual PayMethodOfPayment MethodOfPayment { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
