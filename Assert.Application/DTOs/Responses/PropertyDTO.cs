namespace Assert.Application.DTOs.Responses
{
    public class PropertyDTO
    {
        public decimal? Length { get; set; }
        public int? MaxGuests { get; set; }
        public int? Bedrooms { get; set; }
        public int? Bathrooms { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public AddressDTO Address { get; set; }
        public PropertyTypeDTO Type { get; set; }
        public double? DistanceMeters { get; set; }
    }
}