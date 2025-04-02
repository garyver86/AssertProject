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
        public Task<TlAccommodationType> GetActive(int? accomodationId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TlAccommodationType>> GetActives()
        {
            List<TlAccommodationType> accommodationTypes = await _context.TlAccommodationTypes.Where(x => x != null).ToListAsync();
            return accommodationTypes;
        }
    }
}
