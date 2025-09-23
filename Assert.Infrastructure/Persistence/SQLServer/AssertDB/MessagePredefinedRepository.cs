using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Assert.Domain.Utils;
using Azure;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class MessagePredefinedRepository : IMessagePredefinedRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly MessageValidator _validator;
        public MessagePredefinedRepository(InfraAssertDbContext context)
        {
            _context = context;
            _validator = new MessageValidator();
        }
        public async Task<List<TmMessage>> Get(int conversationId, int page, int pageSize, string orderBy, int userId)
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
                .Where(m => m.ConversationId == conversationId && (m.Conversation.UserIdOne == userId || m.Conversation.UserIdTwo == userId));

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

        public async Task<TmPredefinedMessage> GetByCode(string code)
        {
            // Valida los parámetros de entrada.
            if (code.IsNullOrEmpty())
            {
                throw new ArgumentException("The message code mut be diferent to null and empty.", nameof(code));
            }
            var message = await _context.TmPredefinedMessages.FirstOrDefaultAsync(x => x.Code == code);
            return message;
        }
    }
}
