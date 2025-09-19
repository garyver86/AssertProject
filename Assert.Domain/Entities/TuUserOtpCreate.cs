using System;
using System.Collections.Generic;

namespace Assert.Domain.Entities;

public partial class TuUserOtpCreate
{
    public int UserOtpCreateId { get; set; }

    public string? Email { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? OtpCode { get; set; }

    public string? Status { get; set; }
}
