using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class CancelationPoliciesTypesRepository : ICancelationPoliciesTypesRepository
    {
        private readonly InfraAssertDbContext _context;
        public CancelationPoliciesTypesRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TCancelationPolicyType>> GetActives()
        {
            List<TCancelationPolicyType> amenities = await _context.TCancelationPolicyTypes.Where(x => x.Status == true || x.Status == null).ToListAsync();
            return amenities;
        }

        public async Task<TCancelationPolicyType> GetById(int? cancelationPolicyTypeId)
        {
            TCancelationPolicyType type = await _context.TCancelationPolicyTypes.Where(x => x.CancelationPolicyTypeId == cancelationPolicyTypeId).FirstOrDefaultAsync();
            return type;
        }
    }
}
