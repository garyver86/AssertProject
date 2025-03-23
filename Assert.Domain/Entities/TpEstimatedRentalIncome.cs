using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TpEstimatedRentalIncome
{
    public int EstimatedRentalIncomeId { get; set; }

    public int? PropertySubtypeId { get; set; }

    public int? UnitId { get; set; }

    public int? TimeFrom { get; set; }

    public int? TimeTo { get; set; }

    public int? CurrencyId { get; set; }

    public decimal? IncomeFrom { get; set; }

    public decimal? IncomeTo { get; set; }

    public virtual TCurrency? Currency { get; set; }

    public virtual TpPropertySubtype? PropertySubtype { get; set; }

    public virtual TUnit? Unit { get; set; }
}
