using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingAmenitiesRepository
    {
        Task<List<TlListingAmenity>?> GetByListingRentId(long listingRentId);
    }
}
