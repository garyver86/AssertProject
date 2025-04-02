using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class DiscountTypeRepository : IDiscountTypeRepository
    {
        private readonly InfraAssertDbContext _context;
        public DiscountTypeRepository(InfraAssertDbContext infraAssertDbContext)
        {
            _context = infraAssertDbContext;
        }
        public async Task<List<TDiscountTypeForTypePrice>> GetActives()
        {
            List<TDiscountTypeForTypePrice> discounts = await _context.TDiscountTypeForTypePrices.Where(x => x != null).ToListAsync();
            return discounts;
        }
    }
}
