using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class QuickTypeViewRepository : IQuickTypeViewRepository
    {
        private readonly InfraAssertDbContext _context;
        public QuickTypeViewRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TlQuickTypeView>> GetByTypeId(int viewTypeId, bool onlyActives)
        {
            var result = await _context.TlQuickTypeViews.Where(x => x.ViewTypeId == viewTypeId && x.Status == 1).ToListAsync();
            return result;
        }
    }
}
