using Assert.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Application.DTOs.Responses
{
    public class ComplaintDTO
    {
        public int ComplaintId { get; set; }

        public string ComplainCode { get; set; } = null!;

        public long BookingId { get; set; }

        public int ComplainantUserId { get; set; }

        public int ReportedHostId { get; set; }

        public int ComplaintReasonId { get; set; }

        public string? FreeTextDescription { get; set; }

        public string? ComplaintStatus { get; set; }

        public int? ComplaintStatusId { get; set; }

        public string? ComplaintPriority { get; set; }

        public DateTime? ComplaintDate { get; set; }

        public string? IpAddress { get; set; }

        public string? UserAgent { get; set; }

        public int? AssignedAdminId { get; set; }

        public DateTime? AssignedDate { get; set; }

        public DateTime? ResolutionDate { get; set; }

        public string? ResolutionType { get; set; }

        public string? ResolutionNotes { get; set; }

        public string? InternalNotes { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual BookDTO Booking { get; set; } = null!;

        public virtual UserDTO ComplainantUser { get; set; } = null!;

        public virtual ComplaintReasonDTO ComplaintReason { get; set; } = null!;

        public virtual ComplaintStatusDTO? ComplaintStatusNavigation { get; set; }

        public virtual UserDTO ReportedHost { get; set; } = null!;

        public virtual List<ComplaintEvidenceDTO> TbComplaintEvidences { get; set; }
    }
    public class ComplaintEvidenceDTO
    {
        public int ComplaintEvidenceId { get; set; }
        public int ComplaintId { get; set; }
        public string EvidenceType { get; set; } = null!;
        public string EvidenceUrl { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class ComplaintStatusDTO
    {
        public int ComplaintStatusId { get; set; }

        public string ComplaintStatusCode { get; set; } = null!;

        public bool? IsActive { get; set; }

    }
}
