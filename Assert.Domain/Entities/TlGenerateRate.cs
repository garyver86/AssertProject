using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlGenerateRate
{
    public long GenerateRateId { get; set; }

    public bool IsGenerate { get; set; }

    public bool? IsGenerateAllowUnlimited { get; set; }

    public int? IncludeHour { get; set; }

    public decimal? OverageCharge { get; set; }

    public long? ListingRentId { get; set; }

    public int? SuggestGenerateId { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual TSuggestGenerate? SuggestGenerate { get; set; }
}
