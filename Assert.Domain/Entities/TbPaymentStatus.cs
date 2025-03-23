using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbPaymentStatus
{
    public int PaymentStatusId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<TbBookPayment> TbBookPayments { get; set; } = new List<TbBookPayment>();
}
