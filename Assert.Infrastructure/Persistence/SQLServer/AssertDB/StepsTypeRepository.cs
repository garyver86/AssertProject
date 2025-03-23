using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class StepsTypeRepository : IStepsTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public StepsTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<TlStepsType> Get(int id)
        {
            var result = _context.TlStepsTypes.Where(x => x.StepsTypeId == id).FirstOrDefault();
            return await Task.FromResult(result);
        }

        public async Task<TlStepsType> Get(string code)
        {
            var result = _context.TlStepsTypes.Where(x => x.Code == code).FirstOrDefault();
            return await Task.FromResult(result);
        }
        public async Task<List<TlStepsType>> GetAll()
        {
            var result = await _context.TlStepsTypes.ToListAsync();
            return result;
        }
        public async Task<List<TlStepsType>> GetAllActives()
        {
            var result = await _context.TlStepsTypes.Where(x => x.Status == 1).ToListAsync();
            return result;
        }
    }
}
