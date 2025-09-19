using Assert.Domain.Entities;
using Assert.Domain.Models;
using Assert.Domain.Repositories;
using Assert.Domain.ValueObjects;
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
        public async Task<TmConversation> Create(int renterId, int hostId, long? bookId, long? priceCalculationId, long? listingId)
        {
            if (renterId <= 0)
            {
                throw new ArgumentException("Renter ID must be greater than zero.", nameof(renterId));
            }
            if (hostId <= 0)
            {
                throw new ArgumentException("Host ID must be greater than zero.", nameof(hostId));
            }

            if (renterId == hostId)
            {
                throw new ArgumentException("Renter ID and Host ID cannot be the same.");
            }

            if (bookId > 0)
            {
                //Buscamos una converzación entre estas dos personas con ese mismo bookId
                var existingConversationWithBook = await _context.TmConversations
                    .Include(x => x.PriceCalculation)
                    .Include(x => x.Book)
                    .Include(x => x.ListingRent)
                    .FirstOrDefaultAsync(c => ((c.UserIdOne == renterId && c.UserIdTwo == hostId) ||
                                              (c.UserIdOne == hostId && c.UserIdTwo == renterId)) &&
                                              c.BookId == bookId);
                if (existingConversationWithBook != null)
                {
                    return existingConversationWithBook; // Retorna la conversación existente.
                }
            }
            else if (priceCalculationId > 0)
            {
                //Buscamos una converzación entre estas dos personas con ese mismo priceCalculationId
                var existingConversationWithPriceCalculation = await _context.TmConversations
                    .Include(x => x.PriceCalculation)
                    .Include(x => x.ListingRent)
                    .FirstOrDefaultAsync(c => ((c.UserIdOne == renterId && c.UserIdTwo == hostId) ||
                                              (c.UserIdOne == hostId && c.UserIdTwo == renterId)) &&
                                              c.PriceCalculationId == priceCalculationId);
                if (existingConversationWithPriceCalculation != null)
                {
                    return existingConversationWithPriceCalculation; // Retorna la conversación existente.
                }
            }
            else if (listingId > 0)
            {
                //Buscamos una converzación entre estas dos personas con ese mismo listingId
                var existingConversationWithListing = await _context.TmConversations
                    .FirstOrDefaultAsync(c => ((c.UserIdOne == renterId && c.UserIdTwo == hostId) ||
                                              (c.UserIdOne == hostId && c.UserIdTwo == renterId)) &&
                                              c.ListingRentId == listingId);
                if (existingConversationWithListing != null)
                {
                    return existingConversationWithListing; // Retorna la conversación existente.
                }
            }
            else
            {
                // Verifica si ya existe una conversación entre estos dos usuarios.
                var existingConversation = await _context.TmConversations
                        .FirstOrDefaultAsync(c => (c.UserIdOne == renterId && c.UserIdTwo == hostId) ||
                                                  (c.UserIdOne == hostId && c.UserIdTwo == renterId));
                if (existingConversation != null)
                {
                    return existingConversation; // Retorna la conversación existente.
                }
            }

            if (priceCalculationId > 0)
            {
                var priceCalculation = await _context.PayPriceCalculations
                    .FirstOrDefaultAsync(pc => pc.PriceCalculationId == priceCalculationId);
                if (priceCalculation == null)
                {
                    throw new ArgumentException("Price Calculation ID does not exist.", nameof(priceCalculationId));
                }
                bookId = priceCalculation.BookId;
                listingId = priceCalculation.ListingRentId;
            }
            else if (bookId > 0)
            {
                var booking = await _context.TbBooks
                    .FirstOrDefaultAsync(b => b.BookId == bookId);
                if (booking == null)
                {
                    throw new ArgumentException("Book ID does not exist.", nameof(bookId));
                }
                listingId = booking.ListingRentId;
            }
            else if (listingId > 0)
            {
                var listing = await _context.TlListingRents
                    .FirstOrDefaultAsync(l => l.ListingRentId == listingId);
                if (listing == null)
                {
                    throw new ArgumentException("Listing ID does not exist.", nameof(listingId));
                }
            }

            var conversation = new TmConversation
            {
                UserIdOne = renterId,
                UserIdTwo = hostId,
                CreationDate = DateTime.UtcNow, // Establece la fecha de creación.
                StatusId = 1,
                BookId = bookId,
                PriceCalculationId = priceCalculationId
            };

            _context.TmConversations.Add(conversation);
            await _context.SaveChangesAsync();

            return conversation;
        }

        public async Task<List<TmConversation>> Get(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
            }

            var conversations = await _context.TmConversations
                .Include(x => x.UserIdOneNavigation)
                .Include(x => x.UserIdTwoNavigation)
                .Include(x => x.ListingRent)
                .Include(x => x.Book)
                .Include(x => x.PriceCalculation)
                //.Include(x => x.ListingRent.TpProperties)
                .Include(x => x.ListingRent.TlListingPhotos)
                //.Include(x => x.ListingRent.AccomodationType)
                .Where(c => c.UserIdOne == userId || c.UserIdTwo == userId)
                .Select(c => new TmConversation
                {
                    // Mapear todas las propiedades que necesitas
                    ConversationId = c.ConversationId,
                    UserIdOne = c.UserIdOne,
                    UserIdTwo = c.UserIdTwo,
                    UserIdOneNavigation = c.UserIdOneNavigation,
                    UserIdTwoNavigation = c.UserIdTwoNavigation,
                    //ListingRent = c.ListingRent,
                    Book = c.Book,
                    PriceCalculation = c.PriceCalculation,

                    // Cargar solo la foto principal (asumiendo que hay una propiedad IsPrimary)
                    ListingRent = c.ListingRent != null ? new TlListingRent
                    {
                        ListingRentId = c.ListingRent.ListingRentId,
                        TpProperties = c.ListingRent.TpProperties,
                        AccomodationType = c.ListingRent.AccomodationType,
                        Name = c.ListingRent.Name,
                        Description = c.ListingRent.Description,
                        TlListingPhotos = c.ListingRent.TlListingPhotos.Where(p => p.IsPrincipal != null ? (bool)p.IsPrincipal : false) // Asumiendo que hay un campo IsPrimary
                        .Take(1)
                        .ToList()
                    } : c.ListingRent == null && c.PriceCalculation != null ? new TlListingRent
                    {
                        ListingRentId = c.PriceCalculation.ListingRent.ListingRentId,
                        TpProperties = c.PriceCalculation.ListingRent.TpProperties,
                        AccomodationType = c.PriceCalculation.ListingRent.AccomodationType,
                        Name = c.PriceCalculation.ListingRent.Name,
                        Description = c.PriceCalculation.ListingRent.Description,
                        TlListingPhotos = c.PriceCalculation.ListingRent.TlListingPhotos.Where(p => p.IsPrincipal != null ? (bool)p.IsPrincipal : false) // Asumiendo que hay un campo IsPrimary
                        .Take(1)
                        .ToList()
                    } : null,

                    // Cargar solo el último mensaje
                    TmMessages = c.TmMessages
                .OrderByDescending(m => m.CreationDate)
                .Take(1)
                .ToList()
                })
                .ToListAsync();
            foreach (var con in conversations)
            {
                if (con.Book?.ListingRent != null)
                {
                    con.Book.ListingRent = null;
                }
            }
            return conversations;
        }

        public async Task<TmConversation> GetConversation(long conversationId)
        {
            var conversations = await _context.TmConversations
                .Include(x => x.ListingRent)
                .Include(x => x.Book)
                .Include(x => x.PriceCalculation)
                .Include(x => x.UserIdOneNavigation)
                .Include(x => x.UserIdTwoNavigation)
                .Include(x => x.ListingRent.TpProperties)
                .Include(x => x.ListingRent.TlListingPhotos)
                .Include(x => x.ListingRent.AccomodationType)
                .Where(c => c.ConversationId == conversationId)
                .FirstOrDefaultAsync();

            return conversations;
        }

        public async Task<(List<TmConversation> Conversations, PaginationMetadata pagination)> SearchConversations(ConversationFilter filter)
        {
            if (filter.UserId <= 0)
            {
                throw new ArgumentException("User ID must be greater than zero.", nameof(filter.UserId));
            }

            var query = _context.TmConversations
                .Include(c => c.TmMessages)
                .AsQueryable();

            // Filtrar por estado (archivadas)
            if (filter.statusId.HasValue)
            {
                query = query.Where(c => c.StatusId == filter.statusId);
            }

            // Filtrar por conversaciones no leídas
            if (filter.UnreadOnly.HasValue && filter.UnreadOnly.Value)
            {
                query = query.Where(c => c.TmMessages.Any(m => !m.IsRead));
            }

            // Filtrar por usuario específico
            if (filter.UserId.HasValue)
            {
                query = query.Where(c => c.UserIdOne == filter.UserId.Value || c.UserIdTwo == filter.UserId.Value);
            }

            // Obtener conteo total ANTES de filtrar por keywords
            var totalCount = await query.CountAsync();

            // Aplicar filtro de palabras clave y procesar los resultados
            List<TmConversation> conversations;

            if (filter.Keywords != null && filter.Keywords.Any())
            {
                // Primero obtenemos las conversaciones con paginación
                var conversationQuery = query
                    .OrderByDescending(c => c.TmMessages.Max(m => m.CreationDate))
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize);

                conversations = await conversationQuery.ToListAsync();

                // Filtrar los mensajes dentro de cada conversación
                foreach (var conversation in conversations)
                {
                    conversation.TmMessages = conversation.TmMessages
                        .Where(m => filter.Keywords.Any(keyword =>
                            m.Body != null && m.Body.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                // Remover conversaciones que no tienen mensajes después del filtrado
                conversations = conversations
                    .Where(c => c.TmMessages.Any())
                    .ToList();
            }
            else
            {
                // Si no hay keywords, devolver todas las conversaciones normalmente
                conversations = await query
                    .OrderByDescending(c => c.TmMessages.Max(m => m.CreationDate))
                    .Skip((filter.PageNumber - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToListAsync();
            }

            PaginationMetadata pagination = new PaginationMetadata
            {
                CurrentPage = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalItemCount = totalCount,
                TotalPageCount = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };

            return (conversations, pagination);
        }

        public async Task<TmConversation> SetArchived(long conversationId, int userid, bool isArchived)
        {
            var conversations = await _context.TmConversations
               .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

            if (conversations == null)
            {
                throw new ArgumentException("La converzación no existe.", nameof(conversationId));
            }
            if (conversations.UserIdOne != userid && conversations.UserIdTwo != userid)
            {
                throw new ArgumentException("El usuario no es parte de la converzación.", nameof(userid));
            }
            if (conversations.UserIdOne == userid)
            {
                conversations.UserOneArchived = isArchived;
                conversations.UserOneArchivedDateTime = DateTime.UtcNow;
            }
            else
            {
                conversations.UserTwoArchived = isArchived;
                conversations.UserTwoArchivedDateTime = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return conversations;
        }

        public async Task<TmConversation> SetFeatured(long conversationId, int userid, bool isFeatured)
        {
            var conversations = await _context.TmConversations
               .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

            if (conversations == null)
            {
                throw new ArgumentException("La converzación no existe.", nameof(conversationId));
            }
            if (conversations.UserIdOne != userid && conversations.UserIdTwo != userid)
            {
                throw new ArgumentException("El usuario no es parte de la converzación.", nameof(userid));
            }
            if (conversations.UserIdOne == userid)
            {
                conversations.UserOneFeatured = isFeatured;
                conversations.UserOneFeaturedDateTime = DateTime.UtcNow;
            }
            else
            {
                conversations.UserTwoFeatured = isFeatured;
                conversations.UserTwoFeaturedDateTime = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return conversations;
        }

        public async Task<TmConversation> SetSilent(long conversationId, int userid, bool isSilent)
        {
            var conversations = await _context.TmConversations
               .FirstOrDefaultAsync(x => x.ConversationId == conversationId);

            if (conversations == null)
            {
                throw new ArgumentException("La converzación no existe.", nameof(conversationId));
            }
            if (conversations.UserIdOne != userid && conversations.UserIdTwo != userid)
            {
                throw new ArgumentException("El usuario no es parte de la converzación.", nameof(userid));
            }
            if (conversations.UserIdOne == userid)
            {
                conversations.UserOneSilent = isSilent;
                conversations.UserOneSilentDateTime = DateTime.UtcNow;
            }
            else
            {
                conversations.UserTwoSilent = isSilent;
                conversations.UserTwoSilentDateTime = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
            return conversations;
        }
    }
}
