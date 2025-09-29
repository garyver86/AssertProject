using Assert.Domain.Entities;
using Assert.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB;

public class AdditionalFeeRepository(
    InfraAssertDbContext _context) 
    : IAdditionalFeeRepository
{
    public async Task<List<TlAdditionalFee>> Get()
    {
        List<TlAdditionalFee> additionalFees = await _context.TlAdditionalFees
            .ToListAsync();
        return additionalFees;
    }
}
