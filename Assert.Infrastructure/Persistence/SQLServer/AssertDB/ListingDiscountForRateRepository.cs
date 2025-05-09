using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingDiscountForRateRepository : IListingDiscountForRateRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingDiscountForRateRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingDiscountForRate>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingDiscountForRates.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }
    }
}
