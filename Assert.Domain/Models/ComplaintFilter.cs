using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Models
{
    public class ComplaintFilter
    {
        public string Status { get; set; }
        public int? ReportedHostId { get; set; }
        public int? ComplainantUserId { get; set; }
        public string Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int? UserId { get; set; }
        public int? HostId { get; set; }
    }
}
