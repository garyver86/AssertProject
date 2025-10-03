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
    public async Task<List<TlListingAdditionalFee>> Get(long listingRentId)
    {
        List<TlListingAdditionalFee> additionalFees = await _context.TlListingAdditionalFees
            .Include(x => x.AdditionalFee)
            .Where(x => x.ListingRentId == listingRentId)
            .ToListAsync();
        return additionalFees;
    }
}
