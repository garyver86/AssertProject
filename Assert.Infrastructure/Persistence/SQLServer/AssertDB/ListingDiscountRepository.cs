using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingDiscountRepository : IListingDiscountRepository
    {
        public Task SetDiscounts(long listingRentId, IEnumerable<int>? enumerable)
        {
            throw new NotImplementedException();
        }
    }
}
