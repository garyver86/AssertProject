using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingRentRulesRepository
    {
        Task<List<TlListingRentRule>?> GetByListingRentId(long listingRentId);
    }
}
