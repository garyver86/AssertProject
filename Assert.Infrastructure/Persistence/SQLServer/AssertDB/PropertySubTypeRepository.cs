using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PropertySubTypeRepository : IPropertySubTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public PropertySubTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<TpPropertySubtype> Get(int? subtypeId)
        {
            TpPropertySubtype propertySubtypes = await _context.TpPropertySubtypes.Where(x => x.PropertySubtypeId == subtypeId).FirstOrDefaultAsync();
            return propertySubtypes;
        }

        public async Task<TpPropertySubtype> GetActive(int? subtypeId)
        {
            TpPropertySubtype propertySubtypes = await _context.TpPropertySubtypes.Where(x => x.PropertySubtypeId == subtypeId && x.Status == 1).FirstOrDefaultAsync();
            return propertySubtypes;
        }
        public async Task<List<TpPropertySubtype>> GetActives()
        {
            List<TpPropertySubtype> propertySubtypes = await _context.TpPropertySubtypes.Where(x => x.Status == 1).ToListAsync();
            return propertySubtypes;
        }
    }
}
