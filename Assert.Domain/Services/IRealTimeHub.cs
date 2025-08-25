using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IRealTimeHub
    {
        Task ReceiveMessage(object message);
        Task GroupJoined(string groupName);
        Task GroupLeft(string groupName);
        Task UserConnected(string userId);
        Task UserDisconnected(string userId);
    }
}
