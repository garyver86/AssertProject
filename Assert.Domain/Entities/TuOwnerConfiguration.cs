using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuOwnerConfiguration
{
    public int OwnerConfigurationId { get; set; }

    public string KeyConfiguration { get; set; } = null!;

    public string ConfigurationValue { get; set; } = null!;

    public string? ConfigurationValue2 { get; set; }

    public int UserId { get; set; }

    public virtual TuUser User { get; set; } = null!;
}
