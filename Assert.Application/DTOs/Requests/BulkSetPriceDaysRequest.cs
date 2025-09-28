using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class BulkSetPriceDaysRequest
    {
        public List<DateOnly> Dates { get; set; }
        public long ListingRentId { get; set; }
        public decimal NightPrice { get; set; }
    }
}
