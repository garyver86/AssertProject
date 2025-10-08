using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TComplaintStatus
{
    public int ComplaintStatusId { get; set; }

    public string ComplaintStatusCode { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<TbComplaint> TbComplaints { get; set; } = new List<TbComplaint>();
}
