using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingDiscountForRateRepository
    {
        Task<List<TlListingDiscountForRate>?> GetByListingRentId(long listingRentId);
    }
}
