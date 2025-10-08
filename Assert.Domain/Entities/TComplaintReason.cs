using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TComplaintReason
{
    public int ComplaintReasonId { get; set; }

    public string ComplaintReasonCode { get; set; } = null!;

    public string ReasonDescription { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? RequiresFreeText { get; set; }

    public int? SeverityLevel { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<TbComplaint> TbComplaints { get; set; } = new List<TbComplaint>();
}
