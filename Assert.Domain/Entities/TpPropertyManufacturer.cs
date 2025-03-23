namespace Assert.Domain.Entities;

public partial class TpPropertyManufacturer
{
    public int PropertyManufacturerId { get; set; }

    public int? PropertySubtypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? Status { get; set; }

    public virtual TpPropertySubtype? PropertySubtype { get; set; }

    public virtual ICollection<TpPropertyModel> TpPropertyModels { get; set; } = new List<TpPropertyModel>();
}
