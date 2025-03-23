using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingStepViewRepository : IListingStepViewRepository
    {
        private readonly InfraAssertDbContext _context;
        public ListingStepViewRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<TlListingStepsView> Get(List<long> listingSteps, int viewTypeId)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ViewTypeId == viewTypeId && listingSteps.Contains(x.ListingStepsId ?? 0));
            return result;
        }

        public async Task<TlListingStepsView> Get(int listngStepsViewId)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ListngStepsViewId == listngStepsViewId);
            return result;
        }

        public async Task<TlListingStepsView> Get(long listingRentId, string nextViewCode)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ViewType.Code == nextViewCode && x.ListingSteps.ListingRentId == listingRentId);
            return result;
        }

        public async Task SetEnded(int listngStepsViewId, bool isEnded)
        {
            var result = await _context.TlListingStepsViews.FirstOrDefaultAsync(x => x.ListngStepsViewId == listngStepsViewId);
            result.IsEnded = isEnded;
            _context.SaveChanges();
        }
    }
}
