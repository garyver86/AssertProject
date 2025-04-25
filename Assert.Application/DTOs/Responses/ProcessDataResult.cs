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
    }
}
