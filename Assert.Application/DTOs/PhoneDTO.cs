namespace Assert.Application.DTOs
{
    public class PhoneDTO
    {

        public string? CountryCode { get; set; }

        public string? AreaCode { get; set; }

        public string? Number { get; set; }

        public bool? IsPrimary { get; set; }

        public bool? IsMobile { get; set; }
    }
}