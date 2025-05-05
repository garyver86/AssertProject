using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class UserTypeRepository : IUserTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public UserTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<TuUserType> GetByCode(string code)
        {
            var role = await _context.TuUserTypes
                .Where(x => x.Code == code).FirstOrDefaultAsync();
            return role;
        }
    }
}
