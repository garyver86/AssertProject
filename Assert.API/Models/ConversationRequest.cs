namespace Assert.API.Models
{
    public class ConversationRequest
    {
        public int? HostId { get; set; }
        public int? RenterId { get; set; }
        public long? bookId { get; set; }
        public long? priceCalculationId { get; set; }
        public long? listingId { get; set; }
        public bool isBookingRequest { get; set; }
    }
}
