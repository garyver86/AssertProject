using Assert.Domain.Entities;
using Assert.Domain.Services;

namespace Assert.Infrastructure.ExternalServices
{
    public class SocketIoNotificationDispatcher : INotificationDispatcher
    {
        private readonly ISocketIoServer _socketIoServer;
        private readonly IConnectionManager _connectionManager;

        public SocketIoNotificationDispatcher(
            ISocketIoServer socketIoServer,
            IConnectionManager connectionManager)
        {
            _socketIoServer = socketIoServer;
            _connectionManager = connectionManager;
        }

        public async Task SendNotificationAsync(int userId, TnNotification notification)
        {
            var connections = _connectionManager.GetConnections(userId.ToString());
            if (connections != null && connections.Any())
            {
                foreach (var connectionId in connections)
                {
                    await _socketIoServer.EmitTo(connectionId, "ReceiveNotification", notification);
                }
            }
            //await _socketIoServer.To(userId.ToString()).EmitAsync("ReceiveNotification", notification);//Envía la notificación a una sala
        }

        public async Task UpdateUnreadCount(int userId, int count)
        {
            var connections = _connectionManager.GetConnections(userId.ToString());

            if (connections != null && connections.Any())
            {
                foreach (var connectionId in connections)
                {
                    await _socketIoServer.EmitTo(connectionId, "UpdateUnreadCount", count);
                }
            }

            //await _socketIoServer.To(userId.ToString()).EmitAsync("UpdateUnreadCount", count);
        }
    }
}
