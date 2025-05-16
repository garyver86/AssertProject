using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class AccommodationTypeRepository : IAccommodationTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public AccommodationTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<TlAccommodationType> GetActive(int? accomodationId)
        {
            TlAccommodationType accommodationTypes = await _context.TlAccommodationTypes.Where(x => x.AccommodationTypeId == accomodationId).FirstOrDefaultAsync();
            return accommodationTypes;
        }

        public async Task<List<TlAccommodationType>> GetActives()
        {
            List<TlAccommodationType> accommodationTypes = await _context.TlAccommodationTypes.Where(x => x.Status == 1).ToListAsync();
            return accommodationTypes;
        }
    }
}
