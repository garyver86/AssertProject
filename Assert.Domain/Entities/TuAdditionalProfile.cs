using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuAdditionalProfile
{
    public int AdditionalProfileId { get; set; }

    public int? UserId { get; set; }

    public string? WhatIdo { get; set; }

    public string? WantedToGo { get; set; }

    public string? Pets { get; set; }

    public string? Additional { get; set; }

    public virtual ICollection<TuAdditionalProfileLanguage> TuAdditionalProfileLanguages { get; set; } = new List<TuAdditionalProfileLanguage>();

    public virtual ICollection<TuAdditionalProfileLiveAt> TuAdditionalProfileLiveAts { get; set; } = new List<TuAdditionalProfileLiveAt>();

    public virtual TuUser? User { get; set; }
}
