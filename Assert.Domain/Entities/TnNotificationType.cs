using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TnNotificationType
{
    public int TypeId { get; set; }

    public string Nname { get; set; } = null!;

    public string? Ndescription { get; set; }

    public string Icon { get; set; } = null!;

    public string? Template { get; set; }

    public virtual ICollection<TnNotification> TnNotifications { get; set; } = new List<TnNotification>();
}
