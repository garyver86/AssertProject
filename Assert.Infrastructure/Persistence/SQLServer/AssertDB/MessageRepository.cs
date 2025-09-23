using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Domain.Utils;
using Azure.Core;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class MessageRepository : IMessageRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly MessageValidator _validator;
        public MessageRepository(InfraAssertDbContext context)
        {
            _context = context;
            _validator = new MessageValidator();
        }
        public async Task<List<TmMessage>> Get(long conversationId, int page, int pageSize, string orderBy, int userId)
        {
            // Valida los parámetros de entrada.
            if (page <= 0)
            {
                throw new ArgumentException("Page must be greater than zero.", nameof(page));
            }
            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
            }

            // Inicia la consulta obteniendo los mensajes de la conversación.
            IQueryable<TmMessage> query = _context.TmMessages.Include(x => x.MessageType)
                .Where(m => m.ConversationId == conversationId && (m.Conversation.UserIdOne == userId || m.Conversation.UserIdTwo == userId) && (m.OnlyUserId == null || m.OnlyUserId == userId));

            // Aplica el ordenamiento.
            switch (orderBy?.ToLower())
            {
                case "dateasc":
                    query = query.OrderBy(m => m.CreationDate);
                    break;
                case "datedesc":
                    query = query.OrderByDescending(m => m.CreationDate);
                    break;
                default:
                    query = query.OrderByDescending(m => m.CreationDate); // Orden por defecto
                    break;
            }

            // Aplica la paginación.
            query = query.Skip((page - 1) * pageSize)
                         .Take(pageSize);

            // Ejecuta la consulta y devuelve los resultados.
            return await query.ToListAsync();
        }

        // Obtiene el número de mensajes no leídos de un usuario.
        public async Task<int> GetUnreadCount(int userId)
        {
            // Cuenta los mensajes no leídos del usuario.
            return await _context.TmMessages
                .CountAsync(m => m.UserId != userId && m.IsRead == false);
        }

        // Envía un nuevo mensaje a una conversación.
        public async Task<TmMessage> Send(long conversationId, int? userId, string body, int messageType, int? onlyForUserId)
        {
            // Valida los parámetros de entrada
            if (string.IsNullOrWhiteSpace(body))
            {
                throw new ArgumentException("Message body cannot be empty.", nameof(body));
            }

            var validationResult = _validator.ValidateMessage(body);
            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.Message);
            }

            // Crea el nuevo mensaje.
            var message = new TmMessage
            {
                ConversationId = conversationId,
                UserId = userId,
                Body = body,
                CreationDate = DateTime.UtcNow, // Usa UtcNow para consistencia.
                ReadDate = null, // El mensaje se crea como no leído.
                MessageTypeId = messageType,
                IsRead = false,
                MessageStatusId = 1,
                OnlyUserId = onlyForUserId
            };

            // Agrega el mensaje al contexto y guarda los cambios.
            _context.TmMessages.Add(message);
            await _context.SaveChangesAsync();

            // Devuelve el mensaje creado.
            return message;
        }

        // Marca uno o varios mensajes como leídos.
        public async Task<TmMessage> SetAsRead(long conversationId, List<long>? messageIds)
        {
            if (messageIds == null || messageIds?.Count == 0)
            {
                throw new ArgumentException("At least one message ID must be provided.", nameof(messageIds));
            }

            // Localiza los mensajes a actualizar.
            var messagesToUpdate = await _context.TmMessages
                .Where(m => m.ConversationId == conversationId && messageIds.Contains(m.MessageId))
                .ToListAsync();

            if (messagesToUpdate.Count() == 0)
            {
                throw new Exception("No messages found to update.");
            }

            // Marca los mensajes como leídos.
            foreach (var message in messagesToUpdate)
            {
                message.IsRead = true;
                message.ReadDate = DateTime.UtcNow; // Establece la fecha de lectura.
            }

            // Guarda los cambios en la base de datos.
            await _context.SaveChangesAsync();

            return messagesToUpdate.FirstOrDefault(); //Returns the first updated message
        }

        // Marca uno o varios mensajes como no leídos.
        public async Task<TmMessage> SetAsUnread(long conversationId, List<long>? messageIds)
        {
            if (messageIds == null || messageIds.Count() == 0)
            {
                throw new ArgumentException("At least one message ID must be provided.", nameof(messageIds));
            }

            // Localiza los mensajes a actualizar.
            var messagesToUpdate = await _context.TmMessages
                .Where(m => m.ConversationId == conversationId && messageIds.Contains(m.MessageId))
                .ToListAsync();

            if (messagesToUpdate.Count() == 0)
            {
                throw new Exception("No messages found to update.");
            }

            // Marca los mensajes como no leídos.
            foreach (var message in messagesToUpdate)
            {
                message.IsRead = false;
                message.ReadDate = null; // Establece la fecha de lectura.
            }

            // Guarda los cambios en la base de datos.
            await _context.SaveChangesAsync();
            return messagesToUpdate.FirstOrDefault();
        }
    }
}
