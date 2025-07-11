using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuAdditionalProfileLanguage
{
    public int AdditionalProfileLanguageId { get; set; }

    public int? AdditionalProfileId { get; set; }

    public int? UserId { get; set; }

    public int? LanguageId { get; set; }

    public virtual TuAdditionalProfile? AdditionalProfile { get; set; }

    public virtual TLanguage? Language { get; set; }
}
