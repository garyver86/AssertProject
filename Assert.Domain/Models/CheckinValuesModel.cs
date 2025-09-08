using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class CheckinValuesModel
    {
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public DateTime? MaxCheckIn { get; set; }
        public DateTime? CancellationStart { get; set; }
        public DateTime? CancellationEnd { get; set; }
    }
}
