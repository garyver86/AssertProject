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
        public ListingRentDTO? ListingRent { get; internal set; }
        public BookDTO? Booking { get; internal set; }
        public PayPriceCalculationCompleteDTO? PriceCalculation { get; internal set; }
    }
}
