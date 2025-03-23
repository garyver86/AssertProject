using Assert.Domain.Repositories;

namespace Assert.Infrastructure.Persistence.SQLServer.AssertDB
{
    public class ListingPricingRepository : IListingPricingRepository
    {
        public Task SetPricing(long listingRentId, decimal? pricing, int? currencyId)
        {
            throw new NotImplementedException();
        }
    }
}
