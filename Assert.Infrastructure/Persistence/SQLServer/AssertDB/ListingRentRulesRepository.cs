using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingRentRulesRepository : IListingRentRulesRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingRentRulesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingRentRule>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingRentRules.Where(x => x.ListingId == listingRentId).ToListAsync();
            return result;
        }
    }
}
