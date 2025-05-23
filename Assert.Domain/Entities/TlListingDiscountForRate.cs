﻿using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlListingDiscountForRate
{
    public long ListingDiscountForRate { get; set; }

    public long ListingRentId { get; set; }

    public decimal DiscountCalculated { get; set; }

    public decimal Porcentage { get; set; }

    public int DiscountTypeForTypePriceId { get; set; }

    public bool IsDiscount { get; set; }

    public virtual TDiscountTypeForTypePrice DiscountTypeForTypePrice { get; set; } = null!;

    public virtual TlListingRent ListingRent { get; set; } = null!;
}
