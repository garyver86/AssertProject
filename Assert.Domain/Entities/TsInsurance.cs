using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TsInsurance
{
    public int InsuranceId { get; set; }

    public string InsuranceName { get; set; } = null!;

    public string? InsuranceDescription { get; set; }

    public int CoverageTypeId { get; set; }

    public decimal Price { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<TbBookingInsurance> TbBookingInsurances { get; set; } = new List<TbBookingInsurance>();
}
