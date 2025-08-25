using Assert.Domain.Events;
using Assert.Domain.Interfaces.Notifications;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Domain.Implementation
{
    public class UserNotificationEventHandler : IEventDispatcher
    {
        private readonly ILogger<UserNotificationEventHandler> _logger;

        public UserNotificationEventHandler(ILogger<UserNotificationEventHandler> logger)
        {
            _logger = logger;
        }

        public async Task DispatchAsync(DomainEvent domainEvent)
        {
            switch (domainEvent)
            {
                case UserConnectedEvent connectedEvent:
                    await HandleUserConnected(connectedEvent);
                    break;

                case UserDisconnectedEvent disconnectedEvent:
                    await HandleUserDisconnected(disconnectedEvent);
                    break;

                default:
                    _logger.LogWarning("Evento de dominio no manejado: {EventType}", domainEvent.GetType().Name);
                    break;
            }
        }

        private async Task HandleUserConnected(UserConnectedEvent connectedEvent)
        {
            _logger.LogInformation(
                "Usuario conectado - ID: {UserId}, Nombre: {UserName}, Connection: {ConnectionId}",
                connectedEvent.UserId, connectedEvent.UserName, connectedEvent.ConnectionId);

            // Aquí puedes agregar lógica de negocio adicional
            // como notificar a otros sistemas, actualizar base de datos, etc.
            await Task.CompletedTask;
        }

        private async Task HandleUserDisconnected(UserDisconnectedEvent disconnectedEvent)
        {
            _logger.LogInformation(
                "Usuario desconectado - ID: {UserId}, Nombre: {UserName}, Connection: {ConnectionId}",
                disconnectedEvent.UserId, disconnectedEvent.UserName, disconnectedEvent.ConnectionId);

            // Lógica de negocio para desconexión
            await Task.CompletedTask;
        }
    }
}
