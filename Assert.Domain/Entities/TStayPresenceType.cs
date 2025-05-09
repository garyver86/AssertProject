using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TStayPresenceType
{
    public int StayPrecenseTypeId { get; set; }

    public string StayPrecenseTypeName { get; set; } = null!;

    public string? StayPrecenseTypeDescription { get; set; }

    public virtual ICollection<TlStayPresence> TlStayPresences { get; set; } = new List<TlStayPresence>();
}
