namespace Assert.Domain.Models
{
    public class ListingProcessDataModel
    {
        public long? ListingRentId { get; set; }
        public int Step { get; set; }
        public string ViewCode { get; set; }

        public int? SubtypeId { get; set; }
        public int? AccomodationId { get; set; }
        public ProcessData_AddressModel Address { get; set; }
        public int? MaxGuests { get; set; }
        public int? Bedrooms { get; set; }
        public int? Beds { get; set; }
        public bool? AllDoorsLooked { get; set; }
        public List<ProcessData_SpaceModel> BathroomDetails { get; set; }
        public List<int> StayPrecense { get; set; }
        //--------------------------------------------------

        public List<int> Amenities { get; set; }
        public List<int> FeaturedAmenities { get; set; }
        public List<int> SecurityItems { get; set; }
        public List<ProcessData_PhotoModel> Photos { get; set; }
        public string Title { get; set; }
        public List<int> FeaturedAspects { get; set; }
        public string? Description { get; set; }
        //--------------------------------------------------

        public int? ApprovalPolicyTypeId { get; set; }
        public decimal? Pricing { get; set; }
        public List<ProcessData_DiscountModel> Discounts { get; set; }
        public bool? ExternalCameras { get; set; }
        public bool? NoiseDesibelesMonitor { get; set; }
        public bool? PresenceOfWeapons { get; set; }
        public bool ReviewConfirmatin { get; set; }
        public bool UseTechnicalMessages { get; set; }
        public bool? ListingConfirmation { get; set; }
        public int? CurrencyId { get; internal set; }
        public int Bathrooms { get; internal set; }
    }
    public class ProcessData_AddressModel
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? ZipCode { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }
        public long? CityId { get; set; }
    }
    public class ProcessData_SpaceModel
    {
        public int SpaceTypeId { get; set; }
        public int number { get; set; }
    }
    public class ProcessData_PhotoModel
    {
        public long? PhotoId { get; set; }
        public string Title { get; set; }
        public string Base64 { get; set; }
    }
    public class ProcessData_DiscountModel
    {
        public int dicountTypeId { get; set; }
    }
}
