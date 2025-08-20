using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    internal class AmenitiesRepository : IAmenitiesRepository
    {
        private readonly InfraAssertDbContext _context;
        public AmenitiesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TpAmenitiesType>> GetActives()
        {
            List<TpAmenitiesType> amenities = await _context.TpAmenitiesTypes
                .Where(x => x.Status == true)
                .ToListAsync();
            return amenities;
        }
    }
}
