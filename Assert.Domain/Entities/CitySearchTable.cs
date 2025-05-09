using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class CitySearchTable
{
    public long CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string StateCode { get; set; } = null!;

    public string StateName { get; set; } = null!;

    public string CountyName { get; set; } = null!;
}
