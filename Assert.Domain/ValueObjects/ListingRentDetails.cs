namespace Assert.Domain.ValueObjects
{
    public class ListingRentDetails
    {
        public string CurrentViewCode { get; set; }
        public string PreviousViewCode { get; set; }
        public string NextViewCode { get; set; }
        public int? CurrentStep { get; set; }
        public string CurrentViewName { get; set; }
        public string ListingName { get; set; }
        public long ListingRentId { get; set; }
        public object QuickTipModels { get; set; }
        public object CurrentViewData { get; set; }
    }
}
