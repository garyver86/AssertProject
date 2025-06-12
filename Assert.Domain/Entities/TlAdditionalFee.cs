using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TlAdditionalFee
{
    public int AdditionalFeeId { get; set; }

    public string FeeCode { get; set; } = null!;

    public string DeeDescription { get; set; } = null!;

    public string CalculationType { get; set; } = null!;

    public decimal FeeValue { get; set; }
}
