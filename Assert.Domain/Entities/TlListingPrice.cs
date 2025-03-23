using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingPrice
{
    public long ListingPriceId { get; set; }

    public long? ListingRentId { get; set; }

    public int? ListingPriceOfferId { get; set; }

    public int? TimeUnitId { get; set; }

    public int? CurrencyId { get; set; }

    public decimal? PriceNightly { get; set; }

    public decimal? SecurityDepositPrice { get; set; }

    public virtual TCurrency? Currency { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual ICollection<TlListingDiscountForRate> TlListingDiscountForRates { get; set; } = new List<TlListingDiscountForRate>();
}
