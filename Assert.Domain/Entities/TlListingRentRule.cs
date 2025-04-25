namespace Assert.Domain.Entities;

public partial class TlListingRentRule
{
    public long ListingRulesId { get; set; }

    public long? ListingId { get; set; }

    public int? RuleTypeId { get; set; }

    public bool? Value { get; set; }

    public string? ValueString { get; set; }

    public virtual TlListingRent? Listing { get; set; }

    public virtual TpRuleType? RuleType { get; set; }
}
