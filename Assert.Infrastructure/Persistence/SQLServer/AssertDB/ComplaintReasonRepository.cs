using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ComplaintReasonRepository : IComplaintReasonRepository
    {
        private readonly InfraAssertDbContext _context;
        public ComplaintReasonRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TComplaintReason>> GetAll()
        {
            List<TComplaintReason> complaintReasons = await _context.TComplaintReasons.Where(x => x.IsActive == true || x.IsActive == null).ToListAsync();
            return complaintReasons;
        }
    }
}
