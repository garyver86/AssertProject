using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TnNotificationAction
{
    public int ActionId { get; set; }

    public int NotificationId { get; set; }

    public string ActionType { get; set; } = null!;

    public string ActionUrl { get; set; } = null!;

    public string ActionLabel { get; set; } = null!;

    public bool IsPrimary { get; set; }

    public virtual TnNotification Notification { get; set; } = null!;
}
