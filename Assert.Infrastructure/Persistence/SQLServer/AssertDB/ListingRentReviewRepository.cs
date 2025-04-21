using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    internal class ListingRentReviewRepository : IListingRentReviewRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingRentReviewRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingReview>> GetByListingRent(long listingRentId)
        {
            var result = await _context.TlListingReviews
                .Include(x => x.User)
                .Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }
    }
}
