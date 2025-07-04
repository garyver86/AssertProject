using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class PriceCalculationRequestDTO
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int hostId { get; set; }
    }
}
