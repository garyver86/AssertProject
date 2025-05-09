using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbBookingInsurance
{
    public int BookingInsuranceId { get; set; }

    public long BookingId { get; set; }

    public int InsuranceId { get; set; }

    public decimal Price { get; set; }

    public virtual TbBook Booking { get; set; } = null!;

    public virtual TsInsurance Insurance { get; set; } = null!;
}
