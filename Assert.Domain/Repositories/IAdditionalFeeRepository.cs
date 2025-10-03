using Assert.Domain.Entities;

namespace Assert.Domain.Repositories;

public interface IAdditionalFeeRepository
{
    Task<List<TlAdditionalFee>> Get();
    Task<List<TlListingAdditionalFee>> Get(long listingRentId);
}
