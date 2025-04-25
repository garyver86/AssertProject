using Assert.Domain.Common;
using Assert.Domain.Interfaces.Logging;
using Assert.Domain.Interfaces.Queque;

namespace Assert.Application.Services.Logging;

public class ExceptionLoggerService(IExceptionLogQueue _logQueue, RequestMetadata _metadata)
    : IExceptionLoggerService
{
    public void LogAsync(Exception ex, string action, string module, object data)
    {
        var log = new LogQueueModel
        {
            Exception = ex,
            Action = action,
            Module = module,
            Data = data,
            UserId = _metadata.UserId,
            IpAddress = _metadata.IpAddress,
            BrowseInfo = _metadata.UserAgent
        };

        _logQueue.Enqueue(log);
    }
}

