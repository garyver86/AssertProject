namespace Assert.Domain.Entities;

public partial class TpPropertyAmenity
{
    public long PropertyAmenitiesId { get; set; }

    public long? PropertyId { get; set; }

    public int? AmenitiesTypeId { get; set; }

    public bool? Value { get; set; }

    public string? ValueString { get; set; }

    public virtual TpAmenitiesType? AmenitiesType { get; set; }

    public virtual TpProperty? Property { get; set; }
}
