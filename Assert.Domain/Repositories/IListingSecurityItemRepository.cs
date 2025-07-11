using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingSecurityItemsRepository
    {
        Task<List<TlListingSecurityItem>?> GetByListingRentId(long listingRentId);
        Task SetListingSecurityItems(long listingRentId, List<int> securityItems);
    }
}
