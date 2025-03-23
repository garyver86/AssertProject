using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ViewTypeRepository : IViewTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public ViewTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<TlViewType> Get(int? viewTypeId)
        {
            var result = _context.TlViewTypes.Where(x => x.ViewTypeId == viewTypeId).FirstOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<TlViewType> GetByCode(string? viewCode)
        {
            var result = _context.TlViewTypes.Where(x => x.Code == viewCode).FirstOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<List<TlViewType>> GetByType(int stepsTypeId)
        {
            var result = await _context.TlViewTypes.Where(x => x.StepTypeId == stepsTypeId).ToListAsync();
            return result;
        }

        public async Task<TlViewType> GetParent(int? viewTypeId)
        {
            var aux = await Get(viewTypeId);
            var result = await _context.TlViewTypes.FirstOrDefaultAsync(x => x.ViewTypeId == aux.ViewTypeIdParent);
            return result;
        }

        public async Task<TlViewType> GetPrevious(int? viewTypeId)
        {
            var result = await _context.TlViewTypes.FirstOrDefaultAsync(x => x.ViewTypeIdParent == viewTypeId);
            return result;
        }
    }
}
