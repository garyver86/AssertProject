namespace Assert.Application.DTOs.Responses
{
    public class PropertyDTO
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public AddressDTO Address { get; set; }
        public PropertyTypeDTO Type { get; set; }
        public double? DistanceMeters { get; set; }
        
        public string? CityName { get; set; }
        public string? CountyName { get; set; }
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
    }
}