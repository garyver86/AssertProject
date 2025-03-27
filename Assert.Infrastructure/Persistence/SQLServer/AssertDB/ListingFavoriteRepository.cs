using Assert.Domain.Entities;
using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingFavoriteRepository : IListingFavoriteRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingFavoriteRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task ToggleFavorite(int listingRentId, bool setAsFavorite, int userId)
        {
            TlListingFavorite listing = _context.TlListingFavorites.Where(x => x.ListingRentId == listingRentId && x.UserId == userId).FirstOrDefault();
            if (setAsFavorite)
            {
                if (listing == null)
                {
                    listing = new TlListingFavorite
                    {
                        ListingRentId = listingRentId,
                        UserId = userId,
                        CreateAt = DateTime.UtcNow
                    };
                    await _context.TlListingFavorites.AddAsync(listing);
                }
            }
            else
            {
                if (listing != null)
                {
                    _context.TlListingFavorites.Remove(listing);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
