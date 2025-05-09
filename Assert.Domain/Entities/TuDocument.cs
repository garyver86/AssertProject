using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuDocument
{
    public int DocumentId { get; set; }

    public string DocumentNumber { get; set; } = null!;

    public int DocumentTypeId { get; set; }

    public int UserId { get; set; }

    public string? FileName { get; set; }

    public bool Approved { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int StatusId { get; set; }

    public virtual TuUser User { get; set; } = null!;
}
