using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TnNotification
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public int TypeId { get; set; }

    public long? ListingRentId { get; set; }

    public long? BookingId { get; set; }

    public string Title { get; set; } = null!;

    public string MessageBody { get; set; } = null!;

    public bool IsRead { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? ReadAt { get; set; }

    public virtual TbBook? Booking { get; set; }

    public virtual TlListingRent? ListingRent { get; set; }

    public virtual ICollection<TnNotificationAction> TnNotificationActions { get; set; } = new List<TnNotificationAction>();

    public virtual TnNotificationType Type { get; set; } = null!;

    public virtual TuUser User { get; set; } = null!;
}
