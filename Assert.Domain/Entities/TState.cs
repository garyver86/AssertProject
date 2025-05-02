namespace Assert.Domain.Entities;

public partial class TState
{
    public int StateId { get; set; }

    public int CountryId { get; set; }

    public string? IataCode { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsDisabled { get; set; }

    public virtual TCountry Country { get; set; } = null!;

    public virtual ICollection<TCounty> TCounties { get; set; } = new List<TCounty>();

    public virtual ICollection<TuAddress> TuAddresses { get; set; } = new List<TuAddress>();
}
