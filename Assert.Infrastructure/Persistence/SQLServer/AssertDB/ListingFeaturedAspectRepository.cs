using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingFeaturedAspectRepository : IListingFeaturedAspectRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IListingLogRepository _logRepository;
        public ListingFeaturedAspectRepository(InfraAssertDbContext infraAssertDbContext, IListingLogRepository logRepository)
        {
            _context = infraAssertDbContext;
            _logRepository = logRepository;
        }

        public async Task<List<TlListingFeaturedAspect>?> GetByListingRentId(long listingRentId)
        {
            var result = await _context.TlListingFeaturedAspects.Where(x => x.ListingRentId == listingRentId).ToListAsync();
            return result;
        }
        public async Task SetListingFeaturesAspects(long listingRentId, List<int> featuredAspects)
        {
            var actualAspects = _context.TlListingFeaturedAspects.Where(x => x.ListingRentId == listingRentId).ToList();
            List<int> alreadyExist = new List<int>();
            foreach (var featuredAspect in actualAspects)
            {
                if (!featuredAspects.Contains(featuredAspect.FeaturesAspectTypeId))
                {
                    _context.TlListingFeaturedAspects.Remove(featuredAspect);
                }
                else
                {
                    alreadyExist.Add(featuredAspect.FeaturesAspectTypeId);
                }
            }
            foreach (var featureAspect in featuredAspects)
            {
                if (!alreadyExist.Contains(featureAspect))
                {
                    TlListingFeaturedAspect newAmenity = new TlListingFeaturedAspect
                    {
                        FeaturesAspectTypeId = featureAspect,
                        ListingRentId = listingRentId
                    };
                    _context.TlListingFeaturedAspects.Add(newAmenity);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
