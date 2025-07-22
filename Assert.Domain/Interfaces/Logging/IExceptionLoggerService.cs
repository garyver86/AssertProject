using Assert.Domain.Common.Metadata;

namespace Assert.Domain.Interfaces.Logging;

public interface IExceptionLoggerService
{
    void LogAsync(Exception ex, string action, string module, object data);
}

