using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TCity
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public int CountyId { get; set; }

    public bool? IsDisabled { get; set; }

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public string? NormalizedName { get; set; }

    public virtual TCounty County { get; set; } = null!;

    public virtual ICollection<TpProperty> TpProperties { get; set; } = new List<TpProperty>();

    public virtual ICollection<TpPropertyAddress> TpPropertyAddresses { get; set; } = new List<TpPropertyAddress>();

    public virtual ICollection<TuAdditionalProfileLiveAt> TuAdditionalProfileLiveAts { get; set; } = new List<TuAdditionalProfileLiveAt>();
}
