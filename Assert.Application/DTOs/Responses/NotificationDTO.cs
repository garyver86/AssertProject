using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class NotificationDTO
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

        public List<NotificationActionDTO> TnNotificationActions { get; set; } = new List<NotificationActionDTO>();

        public NotificationTypeDTO Type { get; set; } = null!;
    }

    public class NotificationActionDTO
    {
        public int ActionId { get; set; }

        public int NotificationId { get; set; }

        public string ActionType { get; set; } = null!;

        public string ActionUrl { get; set; } = null!;

        public string ActionLabel { get; set; } = null!;

        public bool IsPrimary { get; set; }
    }

    public class NotificationTypeDTO
    {
        public int TypeId { get; set; }

        public string Nname { get; set; } = null!;

        public string? Ndescription { get; set; }

        public string Icon { get; set; } = null!;

        //public string? Template { get; set; }
    }
}
