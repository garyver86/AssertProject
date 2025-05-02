namespace Assert.Domain.Entities;

public partial class TCounty
{
    public int CountyId { get; set; }

    public string Name { get; set; } = null!;

    public int? StateId { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual TState? State { get; set; }

    public virtual ICollection<TCity> TCities { get; set; } = new List<TCity>();
}
