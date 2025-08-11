using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuAdditionalProfileLiveAt
{
    public int AdditionalProfileLiveAtId { get; set; }

    public int? AdditionalProfileId { get; set; }

    public int? StateId { get; set; }

    public string? CityName { get; set; }

    public string? Location { get; set; }

    public int? CityId { get; set; }

    public virtual TuAdditionalProfile? AdditionalProfile { get; set; }

    public virtual TCity? City { get; set; }

    public virtual TState? State { get; set; }
}
