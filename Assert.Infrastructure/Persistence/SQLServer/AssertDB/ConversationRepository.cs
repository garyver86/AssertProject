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
                .Include(x => x.UserIdOneNavigation)
                .Include(x => x.UserIdTwoNavigation)
                .Include(x => x.ListingRent)
                .Include(x => x.Book)
                .Include(x => x.PriceCalculation)
                .Include(x => x.ListingRent.TpProperties)
                .Include(x => x.ListingRent.TlListingPhotos)
                .Include(x => x.ListingRent.AccomodationType)
                .Where(c => c.UserIdOne == userId || c.UserIdTwo == userId)
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
    }
}
