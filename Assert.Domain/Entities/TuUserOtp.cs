using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserOtp
{
    public int UserOtpId { get; set; }

    public int UserId { get; set; }

    public DateTime? DateLastGen { get; set; }

    public string? OtpCode { get; set; }

    public string? UseOldPassword { get; set; }

    public string? UseNewPassword { get; set; }

    public string? Status { get; set; }

    public virtual TuUser User { get; set; } = null!;
}
