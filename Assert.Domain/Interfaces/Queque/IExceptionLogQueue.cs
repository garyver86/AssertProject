using Assert.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Interfaces.Queque;

public interface IExceptionLogQueue
{
    void Enqueue(LogQueueModel logModel);
    bool TryDequeue(out LogQueueModel logModel);
}
