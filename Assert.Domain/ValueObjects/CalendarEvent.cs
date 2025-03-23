namespace Assert.Domain.ValueObjects
{
    public class CalendarEvent
    {
        public int? id { get; set; }
        public int? listingRentId { get; set; }
        public string ListingName { get; set; }
        public string ListingStatus { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string original_end { get; set; }
        public string color { get; set; }
        public string allDay { get; internal set; }
        public string status { get; set; }
        public int type { get; set; }
        public string description { get; set; }
        public string bookedBy { get; set; }
        public int? bookedById { get; set; }
        public string bookAmount { get; set; }
        public int? bookId { get; set; }
    }
}
