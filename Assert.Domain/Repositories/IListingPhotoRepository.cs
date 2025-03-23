using Assert.Domain.Entities;

namespace Assert.Domain.Repositories
{
    public interface IListingPhotoRepository
    {
        List<TlListingPhoto> GetByListingRent(long listingRentId);
        Task<List<TlListingPhoto>> GetByListingRentId(long listingRentId);
    }
}
