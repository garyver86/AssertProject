using Assert.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Assert.Infrastructure.Common;

namespace Assert.Infrastructure.ExternalServices
{
    public class SignalRService:IRealTimeMessageService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendToUserAsync(string userId, string method, object message)
        {
            await _hubContext.Clients.User(userId).SendAsync(method, message);
        }

        public async Task SendToGroupAsync(string groupName, string method, object message)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, message);
        }

        public async Task SendToAllAsync(string method, object message)
        {
            await _hubContext.Clients.All.SendAsync(method, message);
        }

        public async Task AddToGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.AddToGroupAsync(connectionId, groupName);
        }

        public async Task RemoveFromGroupAsync(string connectionId, string groupName)
        {
            await _hubContext.Groups.RemoveFromGroupAsync(connectionId, groupName);
        }
    }
}
