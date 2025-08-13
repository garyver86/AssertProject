using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class NormalizedLocationInput
    {

        public NormalizedLocationInput(string? country, string? state, string? county, string? city)
        {
            this.Country = country;
            this.State = state;
            this.County = county;
            this.City = city;
        }

        public string? Country { get; set; }
        public string? State { get; set; }
        public string? County { get; set; }
        public string? City { get; set; }
    }
}
