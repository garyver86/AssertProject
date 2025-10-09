using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class ComplaintRequestDTO
    {
        public long BookingId { get; set; }
        public int ComplaintReasonId { get; set; }
        public string FreeTextDescription { get; set; }
    }
}
