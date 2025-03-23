using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CityRepository : ICityRepository
    {
        private readonly InfraAssertDbContext _context;
        public CityRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public TCity GetById(int cityId)
        {
            throw new NotImplementedException();
        }

        public async Task<TCity> GetByListingRentId(long listingRentId)
        {
            var result = (await _context.TpPropertyAddresses.Where(x => x.Property.ListingRentId == listingRentId).FirstOrDefaultAsync())?.City;
            return result;
        }
    }
}
