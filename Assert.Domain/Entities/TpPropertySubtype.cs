using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpPropertySubtype
{
    public int PropertySubtypeId { get; set; }

    public int? PropertyTypeId { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Icon { get; set; }

    public int? Status { get; set; }

    public string? PropertyDescription { get; set; }

    public virtual TpPropertyType? PropertyType { get; set; }

    public virtual ICollection<TpEstimatedRentalIncome> TpEstimatedRentalIncomes { get; set; } = new List<TpEstimatedRentalIncome>();

    public virtual ICollection<TpProperty> TpProperties { get; set; } = new List<TpProperty>();
}
