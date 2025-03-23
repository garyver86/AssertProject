using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class StateRepository : IStateRepository
    {
        private readonly InfraAssertDbContext _context;
        public StateRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<TState> GetByCityId(long cityId)
        {
            var result = (await _context.TCities.Where(x => x.CityId == cityId).FirstOrDefaultAsync())?.State;
            return result;
        }
    }
}
