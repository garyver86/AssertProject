namespace Assert.Domain.Entities;

public partial class TpPropertyRule
{
    public long PropertyRulesId { get; set; }

    public long? PropertyId { get; set; }

    public int? RuleTypeId { get; set; }

    public bool? Value { get; set; }

    public string? ValueString { get; set; }

    public virtual TpProperty? Property { get; set; }

    public virtual TpRuleType? RuleType { get; set; }
}
