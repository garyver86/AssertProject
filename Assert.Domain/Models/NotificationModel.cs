using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class NotificationModel
    {
        public long NotificationId { get; set; }
        public int TypeId { get; set; }
        public string Title { get; set; }
        public string MessageBody { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public long? ListingRentId { get; set; }
        public long? BookingId { get; set; }
        public string? Type { get; set; }
        public List<NotificationActionModel> Actions { get; set; } = new();

        public NotificationModel(TnNotification notification, string notificationType)
        {
            NotificationId = notification.NotificationId;
            TypeId = notification.TypeId;
            Title = notification.Title;
            MessageBody = notification.MessageBody;
            CreatedAt = notification.CreatedAt;
            IsRead = notification.IsRead;
            ListingRentId = notification.ListingRentId;
            BookingId = notification.BookingId;
            Type = notificationType;
            Actions = notification.TnNotificationActions?.Select(a => new NotificationActionModel
            {
                ActionType = a.ActionType,
                ActionLabel = a.ActionLabel,
                ActionUrl = a.ActionUrl,
                IsPrimary = a.IsPrimary
            }).ToList() ?? new List<NotificationActionModel>();
        }
    }

    public class NotificationActionModel
    {
        public string ActionType { get; set; }
        public string ActionLabel { get; set; }
        public string ActionUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}

