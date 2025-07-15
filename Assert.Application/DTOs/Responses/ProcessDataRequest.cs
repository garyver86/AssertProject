using Assert.Domain.Models;

namespace Assert.Application.DTOs.Responses
{
    public class ProcessDataRequest
    {
        public long? ListingRentId { get; set; }
        public int Step { get; set; }
        public string ViewCode { get; set; }

        public int? SubtypeId { get; set; }

        public int? AccomodationId { get; set; }
        public ProcessData_Address Address { get; set; }
        public int? MaxGuests { get; set; }
        public int? Bedrooms { get; set; }
        public int? Beds { get; set; }
        public bool? AllDoorsLooked { get; set; }
        public List<ProcessData_Space> BathroomDetails { get; set; }
        public List<int> StayPrecense { get; set; }
        //--------------------------------------------------

        public List<int> Amenities { get; set; }
        public List<int> FeaturedAmenities { get; set; }
        public List<int> SecurityItems { get; set; }
        public List<ProcessData_Photo> Photos { get; set; }
        public string Title { get; set; }
        public List<int> FeaturedAspects { get; set; }
        public string? Description { get; set; }
        //--------------------------------------------------

        public int? ApprovalPolicyTypeId { get; set; }
        public double? Pricing { get; set; }
        public List<ProcessData_Discount> Discounts { get; set; }
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
    public class ProcessData_Address : AddressDTO
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
    public class ProcessData_Space
    {
        public int SpaceTypeId { get; set; }
        public int number { get; set; }
    }
    public class ProcessData_Photo
    {
        public long? PhotoId { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
    }
    public class ProcessData_Discount
    {
        public int dicountTypeId { get; set; }
        public decimal Price { get; set; }
        public decimal Percentage { get; set; }
    }
}
