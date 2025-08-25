using Assert.Domain.Events;
using Assert.Domain.Interfaces.Notifications;
using Assert.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Assert.Infrastructure.Common
{

    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly ILogger<NotificationHub> _logger;

        public NotificationHub(
            IConnectionManager connectionManager,
            ILogger<NotificationHub> logger)
        {
            _connectionManager = connectionManager;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Usuario no autenticado intentó conectar");
                Context.Abort();
                return;
            }

            var connectionId = Context.ConnectionId;

            // Registrar la conexión
            _connectionManager.AddConnection(userId, connectionId);

            // Unir al usuario a su grupo personalizado
            await Groups.AddToGroupAsync(connectionId, $"user_{userId}");

            _logger.LogInformation("Usuario {UserId} conectado. ConnectionId: {ConnectionId}", userId, connectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var connectionId = Context.ConnectionId;
            var userId = GetUserId();

            // Remover la conexión del manager
            _connectionManager.RemoveConnection(connectionId);

            // Salir del grupo
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.RemoveFromGroupAsync(connectionId, $"user_{userId}");
            }

            _logger.LogInformation("Usuario {UserId} desconectado. ConnectionId: {ConnectionId}", userId, connectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task MarkNotificationAsRead(long notificationId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            await Clients.Caller.SendAsync("NotificationMarkedAsRead", notificationId);
            _logger.LogInformation("Usuario {UserId} marcó notificación {NotificationId} como leída", userId, notificationId);
        }

        // Método para solicitar el conteo de no leídos
        public async Task RequestUnreadCount()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return;
            }

            // Aquí deberías obtener el conteo real de tu servicio
            var unreadCount = 0; // await _notificationService.GetUnreadCountAsync(userId);
            await Clients.Caller.SendAsync("UpdateUnreadCount", unreadCount);
        }

        private string GetUserId()
        {
            return Context.User?.FindFirst("Identifier")?.Value;
        }
    }
}
