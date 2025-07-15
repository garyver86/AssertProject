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
        public int? CurrencyId { get; set; }
        public int? Bathrooms { get; set; }
        public decimal? WeekendPrice { get; set; }
        public int? MinimunNoticeDays { get; set; }
        public int? PreparationDays { get; set; }
        public CheckInPoliciesModel? CheckInPolicies { get; set; }
        public List<int> Rules { get; set; }
        public int? privateBathroom { get; set; }
        public int? privateBathroomLodging { get; set; }
        public int? sharedBathroom { get; set; }
    }
    public class CheckInPoliciesModel
    {
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? Instructions { get; set; }
    }
    public class ProcessData_AddressModel
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? ZipCode { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }
        public int? CityId { get; set; }
        public int? CountyId { get; set; }
        public int? StateId { get; set; }
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
        public string Description { get; set; }
        public int? SpaceTypeId { get; set; }
        public bool? IsPrincipal { get; set; }
    }
    public class ProcessData_DiscountModel
    {
        public int dicountTypeId { get; set; }

        public decimal Price { get; set; }

        public decimal Percentage { get; set; }
    }
}
