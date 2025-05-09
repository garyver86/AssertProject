using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TSuggestGenerate
{
    public int SuggestGenerateId { get; set; }

    public string SuggestIncludeHour { get; set; } = null!;

    public string SuggestionOverCharge { get; set; } = null!;

    public decimal IncludeHourAmount { get; set; }

    public decimal OverChargeAmount { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<TlGenerateRate> TlGenerateRates { get; set; } = new List<TlGenerateRate>();
}
