namespace Assert.Domain.Entities;

public partial class TDistanceRateType
{
    public int DistanceRateTypeId { get; set; }

    public string Unit { get; set; } = null!;

    public string NameUnit { get; set; } = null!;

    public string SuggestInclude { get; set; } = null!;

    public string SuggestOverage { get; set; } = null!;

    public decimal SuggestIncludeAmount { get; set; }

    public decimal SuggestOverageAmount { get; set; }

    public int CountryId { get; set; }

    public string? Code { get; set; }
}
