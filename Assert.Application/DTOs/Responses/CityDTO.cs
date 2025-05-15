namespace Assert.Application.DTOs.Responses
{
    public class CityDTO
    {

        public string City { get; set; }
        public string State { get; set; }
        public string StateCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string County { get; set; }
    }

    public class City2DTO
    {
        public long CityId { get; set; }
        public string Name { get; set; }
    }
    public class CountyDTO
    {
        public long CountyId { get; set; }
        public string Name { get; set; }
        public List<City2DTO> Cities { get; set; } = new List<City2DTO>();
    }

    public class StateDTO
    {
        public long StateId { get; set; }
        public string Name { get; set; }
        public List<CountyDTO> Counties { get; set; } = new List<CountyDTO>();
    }

    public class CountryDTO
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public List<StateDTO> States { get; set; } = new List<StateDTO>();
    }
}