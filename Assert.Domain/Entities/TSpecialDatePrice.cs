﻿using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TSpecialDatePrice
{
    public int SpecialDatePriceId { get; set; }

    public DateTime? InitDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? CurrencyId { get; set; }

    public decimal? PriceNightly { get; set; }

    public decimal? SecurityDepositPrice { get; set; }

    public string? SpecialDateName { get; set; }

    public string? SpecialDateDescription { get; set; }

    public int? StatusId { get; set; }

    public virtual TCurrency? Currency { get; set; }

    public virtual ICollection<TlListingSpecialDatePrice> TlListingSpecialDatePrices { get; set; } = new List<TlListingSpecialDatePrice>();
}
