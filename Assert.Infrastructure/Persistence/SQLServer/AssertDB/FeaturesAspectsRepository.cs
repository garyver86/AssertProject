using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class FeaturedAspectsRepository : IFeaturesAspectsRepository
    {
        private readonly InfraAssertDbContext _context;
        public FeaturedAspectsRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TFeaturedAspectType>> GetActives()
        {
            List<TFeaturedAspectType> amenities = await _context.TFeaturedAspectTypes.Where(x => x.FeaturedAspectStatus == 1).ToListAsync();
            return amenities;
        }
    }
}
