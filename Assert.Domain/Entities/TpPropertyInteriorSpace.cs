namespace Assert.Domain.Entities;

public partial class TpPropertyInteriorSpace
{
    public int PropertyInteriorSpaceId { get; set; }

    public long? PropertyId { get; set; }

    public int? InteriorSpaceTypeId { get; set; }

    public int? Value { get; set; }

    public string? ValueString { get; set; }

    public virtual TpInteriorSpaceType? InteriorSpaceType { get; set; }

    public virtual TpProperty? Property { get; set; }
}
