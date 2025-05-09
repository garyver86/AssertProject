using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TExceptionLog
{
    public long ExceptionLogId { get; set; }

    public string Action { get; set; } = null!;

    public string? StackTrace { get; set; }

    public string Message { get; set; } = null!;

    public string? DataRequest { get; set; }

    public DateTime? DateException { get; set; }

    public string? BrowseInfo { get; set; }

    public string? IpAddress { get; set; }

    public string? Module { get; set; }

    public int? UserId { get; set; }

    public virtual TuUser? User { get; set; }
}
