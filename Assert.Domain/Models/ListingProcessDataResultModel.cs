﻿using Assert.Domain.Entities;

namespace Assert.Domain.Models
{
    public class ListingProcessDataResultModel
    {
        public ListingProcessData_Parametrics Parametrics { get; set; }
        public ListingProcessData_ListingData ListingData { get; set; }
    }
    public class ListingProcessData_Parametrics
    {
        public List<TpPropertySubtype> PropertySubTypes { get; set; }
        public List<TlAccommodationType> AccomodationTypes { get; set; }
        public List<TpAmenitiesType> AmenitiesTypes { get; set; }
        public List<TFeaturedAspectType> FeaturedAspects { get; set; }
        public List<TDiscountTypeForTypePrice> DiscountTypes { get; set; }
        public List<TpSecurityItemType> SecurityItems { get; set; }
        public List<TApprovalPolicyType> ApprovalPolicyType { get; set; }
        public List<TpRuleType> RuleTypes { get; set; }
        public List<TCancelationPolicyType> CancelationPolicyTypes { get; set; }
    }
    public class ListingProcessData_ListingData
    {
        public int? PropertySubTypeId { get; set; }

        public List<TlListingPhoto>? ListingPhotos { get; set; }
        public int? MaxGuests { get; set; }
        public int? Bathrooms { get; set; }
        public int? Bedrooms { get; set; }
        public int? Beds { get; set; }
        public List<TlListingAmenity>? Amenities { get; set; }
        public int? AccomodationTypeId { get; set; }
        public TpPropertyAddress? Address { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public ICollection<TlListingFeaturedAspect>? FeaturedAspects { get; set; }
        public ICollection<TlListingDiscountForRate>? Discounts { get; set; }
        public long ListingRentId { get; set; }
        public string? nextViewCode { get; set; }
        public string? actualViewCode { get; set; }
        public decimal? PriceNightly { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? WeekendNightlyPrice { get; set; }
        public List<TlListingSecurityItem>? SecurityItems { get; set; }
        public int? ApprovalPolicyTypeId { get; set; }
        public int? MinimunNoticeDays { get; set; }
        public int? PreparationDays { get; set; }
        public List<TlListingRentRule>? Rules { get; set; }
        public TlCheckInOutPolicy? TlCheckInOutPolicy { get; set; }
        public int? CancelationPolicyTypeId { get; set; }
        public string? CurrencyCode { get; set; }
        public int? privateBathroom { get; set; }
        public int? privateBathroomLodging { get; set; }
        public int? sharedBathroom { get; set; }
        public TCancelationPolicyType CancelationPolicyType { get; set; }
    }
}
