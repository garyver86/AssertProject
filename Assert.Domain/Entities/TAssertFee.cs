using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TAssertFee
{
    public int AssertFeeId { get; set; }

    public decimal? FeePercent { get; set; }

    public decimal? FeeBase { get; set; }

    public int? CountryId { get; set; }

    public int? StateId { get; set; }

    public int? CountyId { get; set; }

    public int? CityId { get; set; }

    public bool IsEnabled { get; set; }
}
