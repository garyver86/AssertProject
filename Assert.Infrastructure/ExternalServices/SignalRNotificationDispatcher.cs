using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Services;
using Assert.Infrastructure.Common;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.ExternalServices
{
    public class SignalRNotificationDispatcher : INotificationDispatcher
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IConnectionManager _connectionManager;

        public SignalRNotificationDispatcher(
            IHubContext<NotificationHub> hubContext,
            IConnectionManager connectionManager)
        {
            _hubContext = hubContext;
            _connectionManager = connectionManager;
        }

        public async Task SendNotificationAsync(int userId, TnNotification notification)
        {
            var connections = _connectionManager.GetConnections(userId.ToString());

            NotificationModel notificationData = new NotificationModel(notification);
            if (connections != null && connections.Any())
            {
                foreach (var connectionId in connections)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", notificationData);
                }
            }

            // También enviar al grupo del usuario (backup)
            await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notificationData);
        }

        public async Task UpdateUnreadCount(int userId, int count)
        {
            var connections = _connectionManager.GetConnections(userId.ToString());

            if (connections != null && connections.Any())
            {
                foreach (var connectionId in connections)
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("UpdateUnreadCount", count);
                }
            }

            // También enviar al grupo del usuario (backup)
            await _hubContext.Clients.User(userId.ToString()).SendAsync("UpdateUnreadCount", count);
        }
    }
}
