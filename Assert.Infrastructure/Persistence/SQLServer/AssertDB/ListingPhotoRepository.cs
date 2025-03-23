using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPhotoRepository : IListingPhotoRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingPhotoRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public List<TlListingPhoto> GetByListingRent(long listingRentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingPhotos.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }
    }
}
