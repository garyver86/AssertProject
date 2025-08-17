using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class LocationModel
    {
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CountyId { get; set; }
        public int? CityId { get; set; }
    }
}
