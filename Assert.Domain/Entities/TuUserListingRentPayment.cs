using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserListingRentPayment
{
    public int UserListingRentPaymentId { get; set; }

    public int? UserListingRentId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public decimal? Amount { get; set; }

    public int? CurrencyId { get; set; }

    public string? Description { get; set; }

    public virtual TCurrency? Currency { get; set; }

    public virtual TuUserListingRent? UserListingRent { get; set; }
}
