using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Interfaces.Logging;

public interface IExceptionLoggerService
{
    void LogAsync(Exception ex, string action, string module, object data);
}

