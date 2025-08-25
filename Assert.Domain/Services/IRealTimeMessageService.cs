using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Services
{
    public interface IRealTimeMessageService
    {
        Task SendToUserAsync(string userId, string method, object message);
        Task SendToGroupAsync(string groupName, string method, object message);
        Task SendToAllAsync(string method, object message);
        Task AddToGroupAsync(string connectionId, string groupName);
        Task RemoveFromGroupAsync(string connectionId, string groupName);
    }
}
