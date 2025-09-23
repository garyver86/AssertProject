using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ReasonRefusedPriceCalculationRepository : IReasonRefusedPriceCalculationRepository
    {
        private readonly InfraAssertDbContext _context;

        public ReasonRefusedPriceCalculationRepository(InfraAssertDbContext context)
        {
            _context = context;
        }

        public async Task<List<TReasonRefusedPriceCalculation>> GetActives()
        {
            return await _context.TReasonRefusedPriceCalculations
                .ToListAsync();
        }
    }
}
