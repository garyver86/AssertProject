namespace Assert.Domain.Entities;

public partial class TUnit
{
    public int UnitId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Symbol { get; set; }

    public virtual ICollection<TpEstimatedRentalIncome> TpEstimatedRentalIncomes { get; set; } = new List<TpEstimatedRentalIncome>();
}
