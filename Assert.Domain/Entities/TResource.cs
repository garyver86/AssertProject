namespace Assert.Domain.Entities;

public partial class TResource
{
    public long TResourceId { get; set; }

    public string ResourceCode { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string ResourceName { get; set; } = null!;

    public int? ResourceOrder { get; set; }

    public int? UserRegisterId { get; set; }

    public DateTime? DateRegister { get; set; }

    public int? UserUpdate { get; set; }

    public DateTime? DateUpdate { get; set; }
}
