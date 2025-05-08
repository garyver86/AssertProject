using Assert.Domain.Entities;

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
    }
    public class ListingProcessData_ListingData
    {
        internal int? PropertySubTypeId;

        public List<TlListingPhoto> ListingPhotos { get; set; }
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
        public ICollection<TlListingFeaturedAspect> FeaturedAspects { get; set; }
        public ICollection<TlListingDiscountForRate> Discounts { get; set; }
        public long ListingRentId { get; set; }
        public string? nextViewCode { get; set; }
        public string? actualViewCode { get; set; }
    }
}
