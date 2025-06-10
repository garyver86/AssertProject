using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly InfraAssertDbContext _context;
        public ConversationRepository(InfraAssertDbContext context)
        {
            _context = context;
        }
        public async Task<TmConversation> Create(int renterId, int hostId)
        {
            if (renterId <= 0)
            {
                throw new ArgumentException("Renter ID must be greater than zero.", nameof(renterId));
            }
            if (hostId <= 0)
            {
                throw new ArgumentException("Host ID must be greater than zero.", nameof(hostId));
            }

            var conversation = new TmConversation
            {
                UserIdOne = renterId,
                UserIdTwo = hostId,
                CreationDate = DateTime.UtcNow, // Establece la fecha de creación.
                StatusId = 1 // Valor por defecto, ajusta según tu lógica de negocio.
            };

            _context.TmConversations.Add(conversation);
            await _context.SaveChangesAsync();

            return conversation;
        }

        //public async Task<List<TmMessage>> Get(long conversationId, int page, int pageSize, string orderBy)
        //{
        //    if (page <= 0)
        //    {
        //        throw new ArgumentException("Page must be greater than zero.", nameof(page));
        //    }
        //    if (pageSize <= 0)
        //    {
        //        throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));
        //    }

        //    IQueryable<TmMessage> query = _context.TmMessages
        //        .Where(m => m.ConversationId == conversationId);

        //    switch (orderBy?.ToLower())
        //    {
        //        case "dateasc":
        //            query = query.OrderBy(m => m.CreationDate);
        //            break;
        //        case "datedesc":
        //            query = query.OrderByDescending(m => m.CreationDate);
        //            break;
        //        default:
        //            query = query.OrderByDescending(m => m.CreationDate);
        //            break;
        //    }

        //    query = query.Skip((page - 1) * pageSize)
        //                 .Take(pageSize);

        //    return await query.ToListAsync();
        //}

        public async Task<List<TmConversation>> Get(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            }

            var conversations = await _context.TmConversations
                .Include(x=>x.UserIdOneNavigation)
                .Include(x=>x.UserIdTwoNavigation)
                .Where(c => c.UserIdOne == userId || c.UserIdTwo == userId)
                .ToListAsync();

            return conversations;
        }

    }
}
