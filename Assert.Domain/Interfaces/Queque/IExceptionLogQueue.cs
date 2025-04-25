using Assert.Domain.Common;

namespace Assert.Domain.Interfaces.Queque;

public interface IExceptionLogQueue
{
    void Enqueue(LogQueueModel logModel);
    bool TryDequeue(out LogQueueModel logModel);
}
