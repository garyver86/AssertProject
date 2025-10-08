using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbComplaint
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

    public virtual TbBook Booking { get; set; } = null!;

    public virtual TuUser ComplainantUser { get; set; } = null!;

    public virtual TComplaintReason ComplaintReason { get; set; } = null!;

    public virtual TComplaintStatus? ComplaintStatusNavigation { get; set; }

    public virtual TuUser ReportedHost { get; set; } = null!;

    public virtual ICollection<TbComplaintEvidence> TbComplaintEvidences { get; set; } = new List<TbComplaintEvidence>();
}
