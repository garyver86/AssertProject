using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingDiscountForRateRepository
    {
        Task<List<TlListingDiscountForRate>?> GetByListingRentId(long listingRentId);

        Task<string> SetDiscounts(long listingRentId, List<(int, decimal)> discountList);
    }
}
