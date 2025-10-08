using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TbComplaintEvidence
{
    public int ComplaintEvidenceId { get; set; }

    public int ComplaintId { get; set; }

    public string? FileType { get; set; }

    public string FileUrl { get; set; } = null!;

    public string? FileName { get; set; }

    public DateTime? UploadedAt { get; set; }

    public virtual TbComplaint Complaint { get; set; } = null!;
}
