namespace Assert.Application.DTOs.Responses
{
    public class AddressDTO
    {

        public string? ZipCode { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }
        public int? cityId { get; set; }

        public virtual CityDTO City { get; set; }
    }
}