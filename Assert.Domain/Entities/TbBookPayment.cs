using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookPayment
{
    public long BookPaymentId { get; set; }

    public long BookId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public int CurrencyId { get; set; }

    public int FormOfPaymentId { get; set; }

    public int PaymentStatusId { get; set; }

    public string AdditionalData { get; set; } = null!;

    public string? Currency { get; set; }

    public string? FormOfPayment { get; set; }

    public DateTime? DatePayment { get; set; }

    public string? TypePayment { get; set; }

    public virtual TbBook Book { get; set; } = null!;

    public virtual TbPaymentStatus PaymentStatus { get; set; } = null!;
}
