using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Common.Metadata;

public class RequestMetadata
{
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string IsMobile { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public string CorrelationId { get; set; } = string.Empty;

    //from login
    public string UserName { get; set; }
    public int UserId { get; set; }
    public int AccountId { get; set; }
}
