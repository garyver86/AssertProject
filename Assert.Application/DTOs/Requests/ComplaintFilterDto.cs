using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Requests
{
    public class ComplaintFilterDto
    {
        public int? ComplainantUserId { get; set; }
        public int? ReportedHostId { get; set; }
        public int? HostId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public int? UserId { get; set; }
    }
}
