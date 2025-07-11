using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ApprovalPolityTypeRepository : IApprovalPolityTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public ApprovalPolityTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }


        public async Task<List<TApprovalPolicyType>> GetActives()
        {
            List<TApprovalPolicyType> amenities = await _context.TApprovalPolicyTypes.Where(x => x.Status == true || x.Status == null).ToListAsync();
            return amenities;
        }
    }
}
