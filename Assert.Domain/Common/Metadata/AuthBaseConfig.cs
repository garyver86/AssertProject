using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Common.Metadata;

public class AuthBaseConfig
{
    public List<string> ClientIds { get; set; } = new();
    public List<string> Secret { get; set; } = new();
}
