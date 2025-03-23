using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class QuickTipRepository : IQuickTipRepository
    {
        private readonly InfraAssertDbContext _context;
        private readonly IQuickTypeViewRepository _quickTypeViewRepository;
        public QuickTipRepository(InfraAssertDbContext infraAssertDbContext, IQuickTypeViewRepository quickTypeViewRepository)
        {
            _context = infraAssertDbContext;
            _quickTypeViewRepository = quickTypeViewRepository;
        }
        public async Task<List<TQuickTip>> GetByViewType(int viewTypeId, string urlResources)
        {
            List<TlQuickTypeView> views = await _quickTypeViewRepository.GetByTypeId(viewTypeId, true);
            List<int> ids = views.Select(x => x.QuickTipId).ToList();
            var result = await _context.TQuickTips.Where(x => ids.Contains(x.QuickTipId)).ToListAsync();

            result.ForEach(obj => obj.IconLink = urlResources + obj.IconLink);
            return result;
        }
    }
}
