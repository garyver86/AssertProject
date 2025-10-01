using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ConversationDTO
    {
        public long ConversationId { get; set; }

        public int UserIdOne { get; set; }

        public int UserIdTwo { get; set; }

        public int StatusId { get; set; }
        public string StatusCode { get; set; }
        public string Status { get; set; }

        public DateTime? CreationDate { get; set; }
        public UserDTO UserIdOneNavigation { get; set; } = null!;

        public UserDTO UserIdTwoNavigation { get; set; } = null!;
        public ListingRentDTO? ListingRent { get; set; }
        public BookDTO? Booking { get; set; }
        public PayPriceCalculationCompleteDTO? PriceCalculation { get; set; }

        public List<MessageDTO> TmMessages { get; set; }

        public bool? Featured { get; set; }

        public bool? Archived { get; set; }

        public bool? Silent { get; set; }

        public DateTime? FeaturedDateTime { get; set; }

        public DateTime? ArchivedDateTime { get; set; }

        public DateTime? SilentDateTime { get; set; }
        public bool IsUnread { get; set; }

        public long? BookId { get; set; }

        public long? PriceCalculationId { get; set; }

        public long? ListingRentId { get; set; }
    }
}
