using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingAmenitiesRepository : IListingAmenitiesRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingAmenitiesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingAmenity>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingAmenities.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }
    }
}
