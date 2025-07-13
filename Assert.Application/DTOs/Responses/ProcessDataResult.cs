using Assert.Application.DTOs.Responses;
using Assert.Domain.Entities;

namespace Assert.Application.DTOs
{
    public class ProcessDataResult
    {
        public ListingProcessData_Parametrics Parametrics { get; set; }
        public ListingProcessData_ListingData ListingData { get; set; }
    }
    public class ListingProcessData_Parametrics
    {
        public List<PropertyTypeDTO> PropertySubTypes { get; set; }
        public List<AccomodationTypeDTO> AccomodationTypes { get; set; }
        public List<AmenityDTO> AmenitiesTypes { get; set; }
        public List<FeaturedAspectDTO> FeaturedAspects { get; set; }
        public List<DiscountDTO> DiscountTypes { get; set; }
        public List<SecurityItemDTO> SecurityItems { get; set; }
        public List<ApprovalPolicyDTO> ApprovalPolicyType { get; set; }
        public List<RentRuleDTO> RuleTypes { get; set; }
        public List<CancelationPolicyDTO> CancelationPolicyTypes { get; set; }
    }
    public class ListingProcessData_ListingData
    {
        internal int? PropertySubTypeId;

        public List<PhotoDTO> ListingPhotos { get; set; }
        public int? MaxGuests { get; set; }
        public int? Bathrooms { get; set; }
        public int? Bedrooms { get; set; }
        public int? Beds { get; set; }
        public List<AmenityDTO>? Amenities { get; set; }
        public int? AccomodationTypeId { get; set; }
        public AddressDTO? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public ICollection<FeaturedAspectDTO> FeaturedAspects { get; set; }
        public ICollection<DiscountDTO> Discounts { get; set; }
        public long ListingRentId { get; set; }
        public string? nextViewCode { get; set; }
        public string? actualViewCode { get; set; }
        public decimal? PriceNightly { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal? WeekendNightlyPrice { get; set; }
        public List<SecurityItemDTO>? SecurityItems { get; set; }
        public int? ApprovalPolicyTypeId { get; set; }
        public int? MinimunNoticeDays { get; set; }
        public int? PreparationDays { get; set; }
        public List<RentRuleDTO>? Rules { get; set; }
        public CheckInOutPolicyDTO? TlCheckInOutPolicy { get; set; }   
    }
}
