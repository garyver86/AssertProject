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
        public Task<TpPropertySubtype> Get(int? subtypeId)
        {
            throw new NotImplementedException();
        }

        public Task<TpPropertySubtype> GetActive(int? subtypeId)
        {
            throw new NotImplementedException();
        }
        public async Task<List<TpPropertySubtype>> GetActives()
        {
            List<TpPropertySubtype> propertySubtypes = await _context.TpPropertySubtypes.Where(x => x.Status == 1).ToListAsync();
            return propertySubtypes;
        }
    }
}
