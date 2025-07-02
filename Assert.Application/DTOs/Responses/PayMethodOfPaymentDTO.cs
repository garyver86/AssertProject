using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class PayMethodOfPaymentDTO
    {
        public int MethodOfPaymentId { get; set; }
        public string MopName { get; set; } = null!;
        public string? MopDescription { get; set; }
        public string? MopCode { get; set; }
        public bool Active { get; set; }
        public string? UrlIcon { get; set; }
    }
}
