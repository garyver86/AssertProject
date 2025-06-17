using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TDiscountTypeForTypePrice
{
    public int DiscountTypeForTypePriceId { get; set; }

    public string Code { get; set; } = null!;

    public int Days { get; set; }

    public string Question { get; set; } = null!;

    public decimal PorcentageSuggest { get; set; }

    public string SuggestDescription { get; set; } = null!;

    public virtual ICollection<TlCalendarDiscount> TlCalendarDiscounts { get; set; } = new List<TlCalendarDiscount>();

    public virtual ICollection<TlListingDiscountForRate> TlListingDiscountForRates { get; set; } = new List<TlListingDiscountForRate>();
}
