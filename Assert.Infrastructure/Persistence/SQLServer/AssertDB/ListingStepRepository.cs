using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingStepRepository : IListingStepRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingStepRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlListingStep>> GetAll(long listingRentId)
        {
            var result = await _context.TlListingSteps.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }

        public async Task<bool> HasViewNotEnded(long listingStepsId)
        {
            var result = await _context.TlListingStepsViews.Where(x => (x.IsEnded ?? false) == false).FirstOrDefaultAsync() != null;
            return result;
        }
    }
}
