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
    public class SecurityItemsRepository : ISecurityItemsRepository
    {
        private readonly InfraAssertDbContext _context;
        public SecurityItemsRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }

        public async Task<List<TpSecurityItemType>> GetActives()
        {
            List<TpSecurityItemType> amenities = await _context.TpSecurityItemTypes.Where(x => x.Status ?? true).ToListAsync();
            return amenities;
        }
    }
}
