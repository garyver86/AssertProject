using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CountyRepository : ICountyRepository
    {
        private readonly InfraAssertDbContext _context;
        public CountyRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<IEnumerable<TCounty>> Find(string filter)
        {
            return await _context.TCounties
           .Where(co => !(co.IsDisabled ?? false) && co.Name.StartsWith(filter))
           .AsNoTracking()
           .ToListAsync();
        }
    }
}
