using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class NotificationHistoryFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? TypeFilter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
