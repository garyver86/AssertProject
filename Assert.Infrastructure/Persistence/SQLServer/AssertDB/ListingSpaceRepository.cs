using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingSpaceRepository : IListingSpaceRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingSpaceRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingSpace>>? GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingSpaces.Where(x => x.ListingId == listingRentId).ToListAsync();
            return result;
        }
    }
}
