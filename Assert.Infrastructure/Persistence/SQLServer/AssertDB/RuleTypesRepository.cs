using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class RuleTypesRepository : IRulesTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public RuleTypesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TpRuleType>> GetActives()
        {
            List<TpRuleType> amenities = await _context.TpRuleTypes.Where(x => x.Status == true || x.Status == null).ToListAsync();
            return amenities;
        }
    }
}
