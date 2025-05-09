using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TBookingPlatform
{
    public int BookingPlatformId { get; set; }

    public string PlatformName { get; set; } = null!;

    public string PlatformIcon { get; set; } = null!;

    public string PlatformCode { get; set; } = null!;

    public virtual ICollection<TlExternalReference> TlExternalReferences { get; set; } = new List<TlExternalReference>();
}
