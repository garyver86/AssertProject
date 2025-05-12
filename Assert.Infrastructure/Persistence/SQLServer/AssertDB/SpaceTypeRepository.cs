using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class SpaceTypeRepository : ISpaceTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public SpaceTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TSpaceType>> Get()
        {
            var role = await _context.TSpaceTypes
               .Where(x => x.Status == true).ToListAsync();
            return role;
        }
    }
}
