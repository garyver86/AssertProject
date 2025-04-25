using Assert.Domain.Common;
using Assert.Domain.Interfaces.Queque;
using System.Collections.Concurrent;

namespace Assert.Infrastructure.Logging;

public class ExceptionLogQueue : IExceptionLogQueue
{
    private readonly ConcurrentQueue<LogQueueModel> _queue = new();

    public void Enqueue(LogQueueModel logModel)
    {
        _queue.Enqueue(logModel);
    }

    public bool TryDequeue(out LogQueueModel logModel)
    {
        return _queue.TryDequeue(out logModel);
    }
}

