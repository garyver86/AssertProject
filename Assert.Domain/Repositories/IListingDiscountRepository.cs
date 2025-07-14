using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingDiscountRepository
    {
        Task<List<TlListingDiscountForRate>?> Get(long listingRentId);
        Task SetDiscounts(long listingRentId, List<(int, decimal)> discountList);
    }
}
