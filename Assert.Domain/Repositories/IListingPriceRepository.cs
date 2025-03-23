using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingPriceRepository
    {
        TlListingPrice GetByListingRent(long listingRentId);
    }
}
