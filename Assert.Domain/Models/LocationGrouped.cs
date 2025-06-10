using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class CityGrouped
    {
        public int cityId { get; set; }
        public string name { get; set; }
    }

    public class CountyGrouped
    {
        public int countyId { get; set; }
        public string name { get; set; }
        public List<CityGrouped> cities { get; set; } = new List<CityGrouped>(); 
    }

    public class StateGrouped
    {
        public int stateId { get; set; }
        public string name { get; set; }
        public List<CountyGrouped> counties { get; set; } = new List<CountyGrouped>();
    }

    public class CountryGrouped
    {
        public int countryId { get; set; }
        public string name { get; set; }
        public List<StateGrouped> states { get; set; } = new List<StateGrouped>(); 
    }

    public class GroupedLocationResponse
    {
        public List<CountryGrouped> data { get; set; } = new List<CountryGrouped>();
    }
}
