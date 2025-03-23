using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly InfraAssertDbContext _context;
        public PhoneRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TuPhone>> GetByUser(int ownerUserId)
        {
            var result = await _context.TuPhones.Where(x => x.UserId == ownerUserId).ToListAsync();
            return result;
        }
    }
}
