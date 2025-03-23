namespace Assert.Domain.Entities;

public partial class TpPropertyModel
{
    public int PropertyModelId { get; set; }

    public int? PropertyManufacturerId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual TpPropertyManufacturer? PropertyManufacturer { get; set; }

    public virtual ICollection<TpProperty> TpProperties { get; set; } = new List<TpProperty>();
}
